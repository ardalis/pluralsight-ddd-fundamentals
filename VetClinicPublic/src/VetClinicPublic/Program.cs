using System;
using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VetClinicPublic;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Services;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Run RabbitMQ in Docker if it's not already running.");
Console.WriteLine(">> docker run -p 5672:5672 -p 15672:15672 rabbitmq:management");

builder.Services.AddControllersWithViews();

// configure site settings
var siteSettings = builder.Configuration.GetSection("SiteSettings");
builder.Services.Configure<SiteConfiguration>(siteSettings);

// configure email sending
var mailserverConfig = builder.Configuration.GetSection("Mailserver");
builder.Services.Configure<MailserverConfiguration>(mailserverConfig);
builder.Services.AddSingleton<ISendEmail, SmtpEmailSender>();
builder.Services.AddSingleton<ISendConfirmationEmails, ConfirmationEmailSender>();

// configure MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Main).Assembly));

// configure messaging
var messagingConfig = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqConfiguration>(messagingConfig);
builder.Services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

builder.Services.AddMassTransit(x =>
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
var app = builder.Build();

if (builder.Environment.IsDevelopment())
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

app.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Main
{
}
