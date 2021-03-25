using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;

namespace VetClinicPublic.Web.Services
{
  // source: https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/
  public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
  {
    private readonly IConnection _connection;

    public RabbitModelPooledObjectPolicy(
      IOptions<RabbitMqConfiguration> rabbitMqOptions)
    {
      _connection = GetConnection(rabbitMqOptions.Value);
    }

    private IConnection GetConnection(RabbitMqConfiguration settings)
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
