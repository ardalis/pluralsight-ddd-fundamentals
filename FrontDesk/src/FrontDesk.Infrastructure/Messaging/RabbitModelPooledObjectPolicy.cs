using Microsoft.EntityFrameworkCore.Metadata;

namespace FrontDesk.Infrastructure.Messaging
{
  // source: https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/
  public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
  {
    private readonly IConnection _connection;

    public RabbitModelPooledObjectPolicy()
    {
      _connection = GetConnection();
    }

    private IConnection GetConnection()
    {
      var factory = new ConnectionFactory()
      {
        HostName = "localhost", // TODO: Read from config
        UserName = MessagingConstants.Credentials.DEFAULT_USERNAME,
        Password = MessagingConstants.Credentials.DEFAULT_PASSWORD,
        Port = 5672,
        VirtualHost = "/",
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
