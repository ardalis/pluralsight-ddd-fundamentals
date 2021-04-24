using System;
using System.Data.Common;
using FrontDesk.Api;
using FrontDesk.Infrastructure.Data;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace IntegrationTests
{
  // https://docs.microsoft.com/en-us/ef/core/testing/sharing-databases
  public class SharedDatabaseFixture : IDisposable
  {
    private static readonly object _lock = new object();
    private static bool _databaseInitialized;

    public SharedDatabaseFixture()
    {
      Connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=PluralsightDDD.FrontDesk.IntegrationTests;ConnectRetryCount=0");

      Seed();

      Connection.Open();
    }

    public DbConnection Connection { get; }

    public AppDbContext CreateContext(DbTransaction transaction = null)
    {
      var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(Connection).Options, new Mock<IMediator>().Object);

      if (transaction != null)
      {
        context.Database.UseTransaction(transaction);
      }

      return context;
    }

    private void Seed()
    {
      lock (_lock)
      {
        if (!_databaseInitialized)
        {
          using (var context = CreateContext())
          {
            context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            var logger = new LoggerFactory().CreateLogger<AppDbContextSeed>();
            var appDbContextSeed = new AppDbContextSeed(context, logger);
            appDbContextSeed.SeedAsync(new OfficeSettings().TestDate).Wait();
            context.SaveChanges();
          }

          _databaseInitialized = true;
        }
      }
    }

    public void Dispose() => Connection.Dispose();
  }
}
