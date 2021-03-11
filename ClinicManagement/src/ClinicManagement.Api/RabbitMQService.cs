using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PluralsightDdd.SharedKernel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ClinicManagement.Api
{
  public class RabbitMQService : IHostedService
  {
    // Create a wrapper around RabbitMQ consumer similar to this one:
    // https://codereview.stackexchange.com/questions/152117/rabbitmq-wrapper-to-work-in-a-concurrent-environment
    private IModel channel = null;
    private IConnection connection = null;
    private const string hostname = "localhost"; // when running in VS, no docker, rabbitmq running on localhost / or in a container
    //private const string hostname = "host.docker.internal"; // rabbit running on machine; app running in docker
    //private const string hostname = "rabbit1"; // everything in docker via docker-compose
    private const string exchangeName = MessagingConstants.Exchanges.FRONTDESK_CLINICMANAGEMENT_EXCHANGE;
    private const string queuein = MessagingConstants.Queues.FDCM_CLINICMANAGEMENT_IN;
    private const string queueout = MessagingConstants.Queues.FDCM_FRONTDESK_IN;

    // Manually Run RabbitMQ
    // docker run --rm -it --hostname ddd-sample-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management


    // Initiate RabbitMQ and start listening to an input queue
    private void Run()
    {
      // ! Fill in your data here !
      var factory = new ConnectionFactory()
      {
        HostName = hostname,
        // port = 5672, default value
        VirtualHost = "/",
        UserName = MessagingConstants.Credentials.DEFAULT_USERNAME,
        Password = MessagingConstants.Credentials.DEFAULT_PASSWORD
      };

      this.connection = factory.CreateConnection();
      this.channel = this.connection.CreateModel();

      // ! Declare an exchange, need to be updated !
      this.channel.ExchangeDeclare(exchangeName, "direct", 
        durable: true,  
        autoDelete: false,
        arguments: null);

      // A queue to read messages
      this.channel.QueueDeclare(queue: queuein,
                          durable: true,
                          exclusive: false,
                          autoDelete: false,
                          arguments: null);
      this.channel.QueueBind(queuein, exchangeName, routingKey: "in");

      // A queue to write messages
      this.channel.QueueDeclare(queue: queueout,
                          durable: true,
                          exclusive: false,
                          autoDelete: false,
                          arguments: null);
      this.channel.QueueBind(queueout, exchangeName, routingKey: "out");

      this.channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

      Console.WriteLine(" [*] Waiting for messages.");

      var consumer = new EventingBasicConsumer(this.channel);
      consumer.Received += OnMessageReceived;

      this.channel.BasicConsume(queue: queuein,
                          autoAck: false,
                          consumer: consumer);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      // TODO: Wrap in a loop, with try-catch, with retry logic
      this.Run();
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      this.channel.Dispose();
      this.connection.Dispose();
      return Task.CompletedTask;
    }

    private void OnMessageReceived(object model, BasicDeliverEventArgs args)
    {
      var body = args.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      Console.WriteLine(" [x] Received {0}", message);

      //int dots = message.Split('.').Length - 1;

      //// Publish a response
      //string outMessage = "reply:" + message;
      //body = Encoding.UTF8.GetBytes(outMessage);

      //this.channel.BasicPublish(exchange: exchangeName,
      //                     routingKey: "out",
      //                     basicProperties: this.channel.CreateBasicProperties(),
      //                     body: body);
      //Console.WriteLine(" [x] Sent {0}", outMessage);

      //Console.WriteLine(" [x] Done");
      //this.channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
    }
  }
}
