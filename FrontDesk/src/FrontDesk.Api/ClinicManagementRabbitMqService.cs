using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;

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

    public ClinicManagementRabbitMqService(
      IOptions<RabbitMqConfiguration> rabbitMqOptions,
      ILogger<ClinicManagementRabbitMqService> logger,
      IServiceScopeFactory serviceScopeFactory)
    {
      var settings = rabbitMqOptions.Value;
      _logger = logger;
      _serviceScopeFactory = serviceScopeFactory;
      InitializeConnection(settings);
    }

    private void InitializeConnection(RabbitMqConfiguration settings)
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
    private void OnMessageReceived(object model, BasicDeliverEventArgs args)
    {
      var body = args.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      _logger.LogInformation(" [x] Received {0}", message);

      HandleMessage(message);
    }

    private void HandleMessage(string message)
    {
      _logger.LogInformation($"Handling Message: {message}");
      using var doc = JsonDocument.Parse(message);
      var root = doc.RootElement;
      var eventType = root.GetProperty("EventType");
      var entity = root.GetProperty("Entity");
      string insertSQLFormat = "SET IDENTITY_INSERT {0} ON\nINSERT INTO {0} (Id, Name) VALUES (@Id, @Name)\nSET IDENTITY_INSERT {0} OFF";

      using (var scope = _serviceScopeFactory.CreateScope())
      {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if(eventType.GetString() == "Doctor-Created")
        {
          string command = string.Format(insertSQLFormat, "Doctors");
          var idParam = new SqlParameter("@Id", entity.GetProperty("Id").GetInt32());
          var nameParam = new SqlParameter("@Name", entity.GetProperty("Name").GetString());
          db.Database.ExecuteSqlRaw(command, idParam, nameParam);
          _logger.LogInformation(command, "Doctors");
        }
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
