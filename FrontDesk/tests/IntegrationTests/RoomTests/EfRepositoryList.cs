using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.RoomTests
{
  public class EfRepositoryList : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryList(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ListsRoomAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var room = new RoomBuilder().WithDefaultValues().Build();

        var repo1 = new EfRepository<Room>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(room);

        var repo2 = new EfRepository<Room>(Fixture.CreateContext(transaction));
        var rooms = (await repo2.ListAsync()).ToList();

        Assert.True(rooms?.Count > 0);
      }
    }
  }
}
