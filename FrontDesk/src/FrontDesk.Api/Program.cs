using System.Linq;
using System.Reflection;
using BlazorShared;
using FastEndpoints;
using FastEndpoints.Swagger;
using FrontDesk.Api;
using FrontDesk.Api.Hubs;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Infrastructure;
using FrontDesk.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

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
                            builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'));
                            builder.SetIsOriginAllowed(origin => true);
                            //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                            builder.AllowAnyMethod();
                            builder.AllowAnyHeader();
                          });
});

builder.Services
  .AddFastEndpoints()
  .SwaggerDocument(options =>
  {
    options.DocumentSettings = s =>
    {
      s.Title = "My API V1";
    };
  });

var assemblies = new Assembly[]
{
  typeof(Program).Assembly,
  typeof(AppDbContext).Assembly,
  typeof(Appointment).Assembly
};
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assemblies));

builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddMessaging(builder.Configuration);

// use real database
// Requires LocalDB which can be installed with SQL Server Express 2016
// https://www.microsoft.com/en-us/download/details.aspx?id=54284
builder.Services.AddDbContext<AppDbContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

bool isDevelopment = builder.Environment.IsDevelopment();
builder.Services.AddInfrastructureDependencies(isDevelopment);

var app = builder.Build();

await app.SeedDatabaseAsync();

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

app.UseFastEndpoints().UseSwaggerGen();
app.MapHub<ScheduleHub>("/schedulehub");

app.Run();

public partial class Program
{
  public const string CORS_POLICY = "CorsPolicy";
}
