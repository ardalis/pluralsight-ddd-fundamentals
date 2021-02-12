using System.Threading.Tasks;
using FrontDesk.Api;
using FrontDesk.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace IntegrationTests
{
  [Collection("Sequential")]
  public abstract class BaseEfRepoTestFixture : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly CustomWebApplicationFactory<Startup> _factory;
    protected AppDbContext _dbContext;

    protected BaseEfRepoTestFixture(CustomWebApplicationFactory<Startup> factory)
    {
      _factory = factory;
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
      builder.UseSqlite("DataSource=file:memdb1?mode=memory");

      return builder.Options;
    }

    protected async Task<EfRepository> GetRepositoryAsync()
    {
      //var options = CreateInMemoryContextOptions();
      var options = CreateSqlLiteOptions();
      var mockMediator = new Mock<IMediator>();

      _dbContext = new TestContext(options, mockMediator.Object);

      var logger = new LoggerFactory().CreateLogger<AppDbContextSeed>();
      var appDbContextSeed = new AppDbContextSeed(_dbContext, logger);
      await appDbContextSeed.SeedAsync(new OfficeSettings().TestDate);

      return new EfRepository(_dbContext);
    }
  }
}
