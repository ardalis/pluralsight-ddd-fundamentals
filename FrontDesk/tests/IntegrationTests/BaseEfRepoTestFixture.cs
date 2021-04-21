using System;
using System.Data.Common;
using System.Threading.Tasks;
using FrontDesk.Api;
using FrontDesk.Infrastructure.Data;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PluralsightDdd.SharedKernel.Interfaces;
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

  [Collection("Sequential")]
  public abstract class BaseEfRepoTestFixture
  {
    public BaseEfRepoTestFixture()
    {

    }
    protected static DbContextOptions<AppDbContext> CreateInMemoryContextOptions()
    {
      // Create a fresh service provider, and therefore a fresh
      // InMemory database instance.
      var serviceProvider = new ServiceCollection()
          .AddEntityFrameworkInMemoryDatabase()
          .BuildServiceProvider();

      // Create a new options instance telling the context to use an
      // InMemory database and the new service provider.
      var builder = new DbContextOptionsBuilder<AppDbContext>();
      builder.UseInMemoryDatabase("TestFrontDesk")
             .UseInternalServiceProvider(serviceProvider);

      return builder.Options;
    }

    protected DbContextOptions<AppDbContext> CreateSqlLiteOptions()
    {
      var builder = new DbContextOptionsBuilder<AppDbContext>();
      //builder.UseSqlite("DataSource=file:memdb1?mode=memory");

      // real db file
      builder.UseSqlite("");
      return builder.Options;
    }

    protected DbContextOptions<AppDbContext> CreateSqlServerOptions()
    {
      var builder = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlServer("");

      return builder.Options;
    }

    protected AppDbContext GetDbContext()
    {
      //var options = CreateInMemoryContextOptions();
      //var options = CreateSqlLiteOptions();
      var options = CreateSqlServerOptions();
      var mockMediator = new Mock<IMediator>();

      var dbContext = new TestContext(options, mockMediator.Object);
      return dbContext;
    }

    protected EfRepository<T> GetRepository<T>(DbTransaction dbTransaction) where T : class, IAggregateRoot
    {
      return new EfRepository<T>(null);
    }
    // obsolete
    protected async Task<EfRepository<T>> GetRepositoryAsync<T>(DbTransaction dbTransaction = null) where T : class, IAggregateRoot
    {
      return new EfRepository<T>(null);
    }
  }
}
