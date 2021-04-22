using System;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryGetById : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetById(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task GetsByIdClientAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        string name = Guid.NewGuid().ToString();
        var client = new ClientBuilder().WithFullname(name).Build();

        var repo1 = new EfRepository<Client>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(client);

        var repo2 = new EfRepository<Client>(Fixture.CreateContext(transaction));
        var clientFromDb = (await repo2.GetByIdAsync(client.Id));

        Assert.Equal(client.Id, clientFromDb.Id);
        Assert.Equal(client.FullName, clientFromDb.FullName);
      }
    }
  }
}
