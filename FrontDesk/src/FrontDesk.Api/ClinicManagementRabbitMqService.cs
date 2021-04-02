using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Api.Hubs;
using FrontDesk.Infrastructure.Data.Sync;
using FrontDesk.Infrastructure.Messaging;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FrontDesk.Api
{
  public class ClinicManagementRabbitMqService : BackgroundService
  {
    private IModel _channel;
    private IConnection _connection;
    private readonly string _queuein = MessagingConstants.Queues.FDCM_FRONTDESK_IN;
    private readonly string _exchangeName = MessagingConstants.Exchanges.FRONTDESK_CLINICMANAGEMENT_EXCHANGE;
    private readonly ILogger<ClinicManagementRabbitMqService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<ScheduleHub> _scheduleHub;

    public ClinicManagementRabbitMqService(
      IOptions<RabbitMqConfiguration> rabbitMqOptions,
      ILogger<ClinicManagementRabbitMqService> logger,
      IServiceScopeFactory serviceScopeFactory,
      IHubContext<ScheduleHub> scheduleHub)
    {
      var settings = rabbitMqOptions.Value;
      _logger = logger;
      _serviceScopeFactory = serviceScopeFactory;
      _scheduleHub = scheduleHub;
      InitializeConnection(settings);
    }

    private void InitializeConnection(RabbitMqConfiguration settings)
    {
      try
      {
        var factory = new ConnectionFactory
        {
          HostName = settings.Hostname,
          UserName = settings.UserName,
          Password = settings.Password,
          VirtualHost = settings.VirtualHost,
          Port = settings.Port
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_exchangeName, "direct",
                                durable: true,
                                autoDelete: false,
                                arguments: null);

        _channel.QueueDeclare(queue: _queuein,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        string routingKey = "entity-changes";
        _channel.QueueBind(_queuein, _exchangeName, routingKey: "entity-changes");

        _logger.LogInformation($"*** Listening for messages on {_exchangeName}-{routingKey}...");

      }
      catch (System.Exception ex)
      {
        _logger.LogError(ex, settings.ToString());
        Thread.Sleep(5000); // let RabbitMQ service start in Docker Compose
        throw;
      }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      stoppingToken.ThrowIfCancellationRequested();

      var consumer = new EventingBasicConsumer(_channel);
      consumer.Received += OnMessageReceived;

      _channel.BasicConsume(queue: _queuein,
                    autoAck: true,
                    consumer: consumer);

      return Task.CompletedTask;
    }
    private async void OnMessageReceived(object model, BasicDeliverEventArgs args)
    {
      var body = args.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      _logger.LogInformation(" [x] Received {0}", message);

      await HandleMessage(message);
    }

    private async Task HandleMessage(string message)
    {
      _logger.LogInformation($"Handling Message: {message}");

      // TODO: A real app would have better error handling for parsing and routing messages
      using var doc = JsonDocument.Parse(message);
      var root = doc.RootElement;
      var eventType = root.GetProperty("EventType");
      var entity = root.GetProperty("Entity");

      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

      if (eventType.GetString() == "Doctor-Created")
      {
        int id = entity.GetProperty("Id").GetInt32();
        string name = entity.GetProperty("Name").GetString();
        var command = new CreateDoctorCommand
        {
          Id = id,
          Name = name
        };
        await mediator.Send(command);

        string notification = $"New Doctor {name} added in Clinic Management. ";
        await _scheduleHub.Clients.All.SendAsync("ReceiveMessage", notification);
      }
      if (eventType.GetString() == "Client-Updated")
      {
        int id = entity.GetProperty("Id").GetInt32();
        string name = entity.GetProperty("Name").GetString();
        var command = new UpdateClientCommand
        {
          Id = id,
          Name = name
        };
        await mediator.Send(command);

        // TODO: Only send notification if changes occurred
        string notification = $"Client {name} updated in Clinic Management.";
        await _scheduleHub.Clients.All.SendAsync("ReceiveMessage", notification);
      }
      // TODO: Implement other kinds of updates
    }

    public override void Dispose()
    {
      _channel.Close();
      _channel.Dispose();
      _connection.Close();
      _connection.Dispose();
      base.Dispose();
    }
  }
}
