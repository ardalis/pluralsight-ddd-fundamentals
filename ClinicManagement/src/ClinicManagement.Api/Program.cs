using System;
using System.Linq;
using System.Reflection;
using BlazorShared;
using ClinicManagement.Api;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Interfaces;
using ClinicManagement.Infrastructure;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Messaging;
using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// use real database
// Requires LocalDB which can be installed with SQL Server Express 2016
// https://www.microsoft.com/en-us/download/details.aspx?id=54284
builder.Services.AddDbContext<AppDbContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton(typeof(IApplicationSettings), typeof(OfficeSettings));

var baseUrlConfig = new BaseUrlConfiguration();
builder.Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: CORS_POLICY,
                          builder =>
                          {
                            builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'), "localhost:6100", "localhost:6150");
                            builder.SetIsOriginAllowed(origin => true);
                            builder.AllowAnyMethod();
                            builder.AllowAnyHeader();
                          });
});

builder.Services.AddFastEndpoints().SwaggerDocument();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSwaggerGenCustom();

// configure messaging
var messagingConfig = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqConfiguration>(messagingConfig);
builder.Services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

builder.Services.AddMassTransit(x =>
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

var assemblies = new Assembly[]
{
  typeof(Room).Assembly,
  typeof(DefaultInfrastructureModule).Assembly,
  typeof(Program).Assembly,
};
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assemblies));

var isDevelopment = builder.Environment.IsDevelopment();
builder.Services.AddInfrastructureDependencies(isDevelopment);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var hostEnvironment = services.GetService<IWebHostEnvironment>();
  var loggerFactory = services.GetRequiredService<ILoggerFactory>();
  var logger = loggerFactory.CreateLogger<Program>();
  logger.LogInformation($"Starting in environment {hostEnvironment.EnvironmentName}");
  try
  {
    var seedService = services.GetRequiredService<AppDbContextSeed>();
    await seedService.SeedAsync(new OfficeSettings().TestDate);
  }
  catch (Exception ex)
  {
    logger.LogError(ex, "An error occurred seeding the DB.");
  }
}

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

//  if enabled configure docker with
// https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-5.0
//app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(CORS_POLICY);

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
  c.RoutePrefix = string.Empty; // Set Swagger UI to app root
});

app.MapFastEndpoints();
//app.MapHub<ClinicManagementHub>($"/{SignalRConstants.HUB_NAME}");

app.Run();

public partial class Program
{
  public const string CORS_POLICY = "CorsPolicy";
}
