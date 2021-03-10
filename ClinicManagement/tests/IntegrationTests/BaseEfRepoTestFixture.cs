using ClinicManagement.Infrastructure.Data;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PluralsightDdd.SharedKernel.Interfaces;
using Xunit;

namespace IntegrationTests
{
  [Collection("Sequential")]
  public abstract class BaseEfRepoTestFixture
  {
    protected AppDbContext _dbContext;
    protected readonly SqliteConnection _connection;

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
      builder.UseInMemoryDatabase("TestClinicManagement")
             .UseInternalServiceProvider(serviceProvider);

      return builder.Options;
    }

    protected DbContextOptions<AppDbContext> CreateSqlLiteOptions()
    {
      var builder = new DbContextOptionsBuilder<AppDbContext>();
      builder.UseSqlite("DataSource=file:memdb1?mode=memory");

      return builder.Options;
    }

    protected EfRepository<T> GetRepository<T>() where T : class, IAggregateRoot
    {
      //var options = CreateInMemoryContextOptions();
      var options = CreateSqlLiteOptions();
      var mockMediator = new Mock<IMediator>();

      _dbContext = new TestContext(options, mockMediator.Object);
      return new EfRepository<T>(_dbContext);
    }
  }
}
