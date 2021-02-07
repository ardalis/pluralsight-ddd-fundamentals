using System;
using FrontDesk.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Api
{
  public class Program
  {
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
      var host = CreateHostBuilder(args)
                  .Build();

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
          var catalogContext = services.GetRequiredService<AppDbContext>();
          await AppDbContextSeed.SeedAsync(catalogContext, loggerFactory, new OfficeSettings().TestDate);
        }
        catch (Exception ex)
        {
          var logger = loggerFactory.CreateLogger<Program>();
          logger.LogError(ex, "An error occurred seeding the DB.");
        }
      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
