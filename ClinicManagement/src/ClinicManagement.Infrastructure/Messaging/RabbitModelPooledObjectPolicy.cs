using System;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ClinicManagement.Infrastructure.Messaging
{
  // source: https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/
  public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
  {
    private readonly IConnection _connection;
    private readonly ILogger<RabbitModelPooledObjectPolicy> _logger;

    public RabbitModelPooledObjectPolicy(
      IOptions<RabbitMqConfiguration> rabbitMqOptions,
      ILogger<RabbitModelPooledObjectPolicy> logger)
    {
      _connection = GetConnection(rabbitMqOptions.Value);
      _logger = Guard.Against.Null(logger, nameof(logger));
    }

    private IConnection GetConnection(RabbitMqConfiguration settings)
    {
      Guard.Against.Null(settings, nameof(settings));
      try
      {
        var factory = new ConnectionFactory()
        {
          HostName = settings.Hostname,
          UserName = settings.UserName,
          Password = settings.Password,
          Port = settings.Port,
          VirtualHost = settings.VirtualHost,
        };

        return factory.CreateConnection();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to connect to RabbitMQ", settings);
        throw;
      }
    }

    public IModel Create()
    {
      return _connection.CreateModel();
    }

    public bool Return(IModel obj)
    {
      if (obj.IsOpen)
      {
        return true;
      }
      else
      {
        obj?.Dispose();
        return false;
      }
    }
  }
}
