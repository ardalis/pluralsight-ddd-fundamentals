using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic
{
  public class FrontDeskRabbitMqService : BackgroundService
  {
    private IModel _channel;
    private IConnection _connection;
    private readonly string _queuein = MessagingConstants.Queues.FDVCP_VETCLINICPUBLIC_IN;
    private readonly string _exchangeName = MessagingConstants.Exchanges.FRONTDESK_VETCLINICPUBLIC_EXCHANGE;
    private readonly ILogger<FrontDeskRabbitMqService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FrontDeskRabbitMqService(
      IOptions<RabbitMqConfiguration> rabbitMqOptions,
      ILogger<FrontDeskRabbitMqService> logger,
      IServiceScopeFactory serviceScopeFactory)
    {
      var settings = rabbitMqOptions.Value;
      _logger = logger;
      _serviceScopeFactory = serviceScopeFactory;
      InitializeConnection(settings);
    }

    private void InitializeConnection(RabbitMqConfiguration settings)
    {
      try
      {
        _logger.LogInformation($"Connecting to RabbitMQ with {settings}");
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

        string routingKey = "appointment-scheduled";
        _channel.QueueBind(_queuein, _exchangeName, routingKey: routingKey);

        _logger.LogInformation($"*** Listening for messages on {_exchangeName}-{routingKey}...");
      }
      catch(Exception ex)
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

      await HandleMessage(message);
    }















    private async Task HandleMessage(string message)
    {
      _logger.LogInformation($"Handling Message: {message}");
      using var doc = JsonDocument.Parse(message);
      var root = doc.RootElement;
      var eventType = root.GetProperty("EventType");

      using var scope = _serviceScopeFactory.CreateScope();
      var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

      if(eventType.GetString() == "AppointmentScheduledIntegrationEvent")
      {
        var command = new SendAppointmentConfirmationCommand()
        {
          AppointmentId = root.GetProperty("AppointmentId").GetGuid(),
          AppointmentType = root.GetProperty("AppointmentType").GetString(),
          ClientEmailAddress = root.GetProperty("ClientEmailAddress").GetString(),
          ClientName = root.GetProperty("ClientName").GetString(),
          DoctorName = root.GetProperty("DoctorName").GetString(),
          PatientName = root.GetProperty("PatientName").GetString(),
          AppointmentStartDateTime = root.GetProperty("AppointmentStartDateTime").GetDateTime()
        };
      await mediator.Send(command);
      }
      else
      {
        throw new Exception($"Unknown message type: {eventType.GetString()}");
      }
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
