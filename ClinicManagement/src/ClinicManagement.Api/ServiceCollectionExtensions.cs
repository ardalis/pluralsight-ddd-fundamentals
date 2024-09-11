using ClinicManagement.Core.Interfaces;
using ClinicManagement.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Api
{
  public static class ServiceCollectionExtensions
  {
    public static void AddMessaging(this IServiceCollection services, ConfigurationManager configuration)
    {
      var messagingConfig = configuration.GetSection("RabbitMq");
      services.Configure<RabbitMqConfiguration>(messagingConfig);
      services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

      services.AddMassTransit(x =>
      {
        var rabbitMqConfiguration = messagingConfig.Get<RabbitMqConfiguration>();
        x.SetKebabCaseEndpointNameFormatter();

        x.UsingRabbitMq((context, cfg) =>
        {
          var port = (ushort)rabbitMqConfiguration.Port;
          cfg.Host(rabbitMqConfiguration.Hostname, port, rabbitMqConfiguration.VirtualHost, h =>
          {
            h.Username(rabbitMqConfiguration.UserName);
            h.Password(rabbitMqConfiguration.Password);
          });

          cfg.ConfigureEndpoints(context);
        });
      });
    }
  }
}
