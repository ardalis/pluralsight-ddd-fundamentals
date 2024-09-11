using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Services;

namespace VetClinicPublic
{
  public static class ServiceCollectionExtensions
  {
    public static void ConfigureSiteSettings(this IServiceCollection services, ConfigurationManager configuration)
    {
      var siteSettings = configuration.GetSection("SiteSettings");
      services.Configure<SiteConfiguration>(siteSettings);
    }

    public static void ConfigureEmailSending(this IServiceCollection services, ConfigurationManager configuration)
    {
      var mailserverConfig = configuration.GetSection("Mailserver");
      services.Configure<MailserverConfiguration>(mailserverConfig);
      services.AddSingleton<ISendEmail, SmtpEmailSender>();
      services.AddSingleton<ISendConfirmationEmails, ConfirmationEmailSender>();

      services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Main).Assembly));
    }

    public static void AddMessaging(this IServiceCollection services, ConfigurationManager configuration)
    {
      var messagingConfig = configuration.GetSection("RabbitMq");
      services.Configure<RabbitMqConfiguration>(messagingConfig);
      services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

      services.AddMassTransit(x =>
      {
        var rabbitMqConfiguration = messagingConfig.Get<RabbitMqConfiguration>();
        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumers(Assembly.GetExecutingAssembly());

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
