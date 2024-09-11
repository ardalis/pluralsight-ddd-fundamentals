using System;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Api
{
  public static class WebApplicationExtensions
  {
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
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
          //var catalogContext = services.GetRequiredService<AppDbContext>();
          await seedService.SeedAsync(new OfficeSettings().TestDate);
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "An error occurred seeding the DB.");
        }
      }
    }
  }
}
