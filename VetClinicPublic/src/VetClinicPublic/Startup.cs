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

      // configure site settings
      var siteSettings = Configuration.GetSection("SiteSettings");
      services.Configure<SiteConfiguration>(siteSettings);

      // configure email sending
      var mailserverConfig = Configuration.GetSection("Mailserver");
      services.Configure<MailserverConfiguration>(mailserverConfig);
      services.AddSingleton<ISendEmail, SmtpEmailSender>();
      services.AddSingleton<ISendConfirmationEmails, ConfirmationEmailSender>();


      // configure MediatR
      services.AddMediatR(typeof(Startup).Assembly);

      // configure messaging
      var messagingConfig = Configuration.GetSection("RabbitMq");
      var messagingSettings = messagingConfig.Get<RabbitMqConfiguration>();
      services.Configure<RabbitMqConfiguration>(messagingConfig);
      services.AddSingleton<IMessagePublisher, RabbitMessagePublisher>();
      services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
      services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
      if (messagingSettings.Enabled)
      {
        services.AddHostedService<FrontDeskRabbitMqService>();
      }
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
