using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Services;
using MediatR;

namespace VetClinicPublic
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllersWithViews();
      services.AddSingleton<ISendConfirmationEmails, SmtpConfirmationEmailSender>();
      services.AddSingleton<IMessagePublisher, RabbitMessagePublisher>();
      services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
      services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

      // configure MediatR
      services.AddMediatR(typeof(Startup).Assembly);

      // configure messaging
      var messagingConfig = Configuration.GetSection("RabbitMq");
      var messagingSettings = messagingConfig.Get<RabbitMqConfiguration>();
      services.Configure<RabbitMqConfiguration>(messagingConfig);
      if (messagingSettings.Enabled)
      {
        services.AddHostedService<FrontDeskRabbitMqService>();
        //services.AddHostedService<VetClinicPublicRabbitMqService>();
      }


      //// https://github.com/AntonyVorontsov/RabbitMQ.Client.Core.DependencyInjection/tree/master/examples/Examples.AdvancedConfiguration
      //var rabbitMqConsumerSection = Configuration.GetSection("RabbitMqConsumer");
      //var rabbitMqProducerSection = Configuration.GetSection("RabbitMqProducer");

      //var producingExchangeSection = Configuration.GetSection("ProducingExchange");
      //var consumingExchangeSection = Configuration.GetSection("ConsumingExchange");

      //services
      //    .AddRabbitMqConsumingClientSingleton(rabbitMqConsumerSection)
      //    .AddRabbitMqProducingClientSingleton(rabbitMqProducerSection)
      //    .AddProductionExchange("exchange.to.send.messages.only", producingExchangeSection)
      //    .AddConsumptionExchange("consumption.exchange", consumingExchangeSection);
      //    //.AddMessageHandlerSingleton<CustomMessageHandler>("routing.key");

      //services.AddHostedService<ConsumingHostedService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
