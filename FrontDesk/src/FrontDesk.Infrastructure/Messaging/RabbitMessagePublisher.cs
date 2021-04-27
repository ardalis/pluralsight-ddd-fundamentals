using System;
using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using FrontDesk.Core.Events.IntegrationEvents;
using FrontDesk.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;

namespace FrontDesk.Infrastructure.Messaging
{
  public class RabbitMessagePublisher : IMessagePublisher
  {
    private readonly DefaultObjectPool<IModel> _objectPool;
    private readonly ILogger<RabbitMessagePublisher> _logger;

    public RabbitMessagePublisher(IPooledObjectPolicy<IModel> objectPolicy,
      ILogger<RabbitMessagePublisher> logger)
    {
      _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
      _logger = logger;
    }

    public void Publish(AppointmentScheduledIntegrationEvent eventToPublish)
    {
      Guard.Against.Null(eventToPublish, nameof(eventToPublish));

      var channel = _objectPool.Get();

      object message = (object)eventToPublish;
      try
      {
        string exchangeName = MessagingConstants.Exchanges.FRONTDESK_VETCLINICPUBLIC_EXCHANGE;
        channel.ExchangeDeclare(exchangeName, "direct", true, false, null);

        var messageString = JsonSerializer.Serialize(message);
        var sendBytes = Encoding.UTF8.GetBytes(messageString);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(
          exchange: exchangeName,
          routingKey: "appointment-scheduled",
          basicProperties: properties,
          body: sendBytes);
        _logger.LogInformation($"Sending appt scheduled event: {messageString}");
      }
      finally
      {
        _objectPool.Return(channel);
      }
    }
  }
}
