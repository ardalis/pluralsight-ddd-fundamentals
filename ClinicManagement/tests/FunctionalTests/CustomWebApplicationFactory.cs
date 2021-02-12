using System;
using System.Linq;
using ClinicManagement.Api;
using ClinicManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunctionalTests
{
  public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
  {
    private readonly string _connectionString = "Data Source=functionaltests.db";
    private readonly SqliteConnection _connection;

    public CustomWebApplicationFactory()
    {
      _connection = new SqliteConnection(_connectionString);
      _connection.Open();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
      var host = builder.Build();

      // Get service provider.
      var serviceProvider = host.Services;

      // Create a scope to obtain a reference to the database
      // context (AppDbContext).
      using (var scope = serviceProvider.CreateScope())
      {
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<AppDbContext>();

        var logger = scopedServices
            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

        // Ensure the database is created.
        db.Database.EnsureCreated();

        try
        {
          // Seed the database with test data.
          var seedService = scopedServices.GetRequiredService<AppDbContextSeed>();
          seedService.SeedAsync(new OfficeSettings().TestDate).Wait();
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "An error occurred seeding the " +
                              $"database with test messages. Error: {ex.Message}");
        }
      }

      host.Start();
      return host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder
          //.UseSolutionRelativeContentRoot("tests/FunctionalTests")
          .ConfigureServices(services =>
          {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
              services.Remove(descriptor);
            }

            services
              .AddEntityFrameworkSqlite()
                .AddDbContext<AppDbContext>(options =>
                {
                  options.UseSqlite(_connection);
                  options.UseInternalServiceProvider(services.BuildServiceProvider());
                });

            // services.AddScoped<IMediator, NoOpMediator>();
          });
    }
  }
}
