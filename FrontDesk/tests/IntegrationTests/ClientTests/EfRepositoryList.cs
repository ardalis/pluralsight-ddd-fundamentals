using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryList : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryList(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ListsClientAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var client = new ClientBuilder().WithFullname(ClientBuilder.DEFAULT_FULL_NAME).Build();

        var repo1 = new EfRepository<Client>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(client);

        var repo2 = new EfRepository<Client>(Fixture.CreateContext(transaction));
        var clients = (await repo2.ListAsync()).ToList();

        Assert.True(clients?.Count > 0);
      }
    }
  }
}
