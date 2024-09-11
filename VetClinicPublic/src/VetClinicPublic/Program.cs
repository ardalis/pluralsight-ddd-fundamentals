using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VetClinicPublic;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Run RabbitMQ in Docker if it's not already running.");
Console.WriteLine(">> docker run -p 5672:5672 -p 15672:15672 rabbitmq:management");

builder.Services.AddControllersWithViews();

var configuration = builder.Configuration;
builder.Services.ConfigureSiteSettings(configuration);
builder.Services.ConfigureEmailSending(configuration);
builder.Services.AddMessaging(configuration);

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
