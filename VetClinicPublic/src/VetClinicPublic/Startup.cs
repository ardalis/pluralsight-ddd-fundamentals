using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Services;

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

      // configure site settings
      var siteSettings = Configuration.GetSection("SiteSettings");
      services.Configure<SiteConfiguration>(siteSettings);

      // configure email sending
      var mailserverConfig = Configuration.GetSection("Mailserver");
      services.Configure<MailserverConfiguration>(mailserverConfig);
      services.AddSingleton<ISendEmail, SmtpEmailSender>();
      services.AddSingleton<ISendConfirmationEmails, ConfirmationEmailSender>();


      // configure MediatR
      services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Startup).Assembly));

      // configure messaging
      var messagingConfig = Configuration.GetSection("RabbitMq");
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
      // disabled because of docker setup required
      //app.UseHttpsRedirection();
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
