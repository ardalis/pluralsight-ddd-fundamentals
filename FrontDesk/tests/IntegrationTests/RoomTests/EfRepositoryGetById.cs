using System;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.RoomTests
{
  public class EfRepositoryGetById : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetById(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task GetsByIdRoomAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        string name = Guid.NewGuid().ToString();
        var room = new RoomBuilder().WithName(name).Build();

        var repo1 = new EfRepository<Room>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(room);

        var repo2 = new EfRepository<Room>(Fixture.CreateContext(transaction));
        var roomFromDb = (await repo2.GetByIdAsync(room.Id));

        Assert.Equal(room.Id, roomFromDb.Id);
        Assert.Equal(room.Name, roomFromDb.Name);
      }
    }
  }
}
