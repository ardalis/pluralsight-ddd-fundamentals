using System;
using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.Extensions.ObjectPool;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic.Web.Services
{
  public class RabbitMessagePublisher : IMessagePublisher
  {
    private readonly DefaultObjectPool<IModel> _objectPool;

    public RabbitMessagePublisher(IPooledObjectPolicy<IModel> objectPolicy)
    {
      _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
    }

    public void Publish(AppointmentConfirmLinkClickedIntegrationEvent eventToPublish)
    {
      Guard.Against.Null(eventToPublish, nameof(eventToPublish));

      var channel = _objectPool.Get();

      object message = (object)eventToPublish;
      try
      {
        string exchangeName = MessagingConstants.Exchanges.FRONTDESK_VETCLINICPUBLIC_EXCHANGE;
        channel.ExchangeDeclare(exchangeName, "direct", true, false, null);

        var sendBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(
          exchange: exchangeName,
          routingKey: "appointment-confirmation",
          basicProperties: properties,
          body: sendBytes);
      }
      finally
      {
        _objectPool.Return(channel);
      }
    }
  }
}
