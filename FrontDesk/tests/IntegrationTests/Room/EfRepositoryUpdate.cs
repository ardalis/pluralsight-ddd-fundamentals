using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Room
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryUpdate()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task UpdatesRoomAfterAddingIt()
    {
      var id = 2;
      var name = "changed";

      var room = await AddRoom(id);

      room.UpdateName(name);
      await _repository.UpdateAsync<FrontDesk.Core.Aggregates.Room, int>(room);

      var updatedRoom = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Room, int>(id);

      Assert.Equal(name, updatedRoom.Name);
    }

    private async Task<FrontDesk.Core.Aggregates.Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Room, int>(room);

      return room;
    }
  }
}
