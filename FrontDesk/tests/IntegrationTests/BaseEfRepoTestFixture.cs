using FrontDesk.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace IntegrationTests
{
  [Collection("Sequential")]
  public abstract class BaseEfRepoTestFixture
  {
    protected AppDbContext _dbContext;

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
      builder.UseSqlite("Data Source=test.db");

      return builder.Options;
    }


    protected EfRepository GetRepository()
    {
      //var options = CreateInMemoryContextOptions();
      var options = CreateSqlLiteOptions();
      var mockMediator = new Mock<IMediator>();

      _dbContext = new AppDbContext(options, mockMediator.Object);

      return new EfRepository(_dbContext);
    }
  }
}
