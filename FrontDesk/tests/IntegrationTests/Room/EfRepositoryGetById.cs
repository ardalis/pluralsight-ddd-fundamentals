using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Room
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task GetsByIdRoomAfterAddingIt()
    {
      var id = 9;
      var room = await AddRoom(id);

      var newRoom = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Room, int>(id);

      Assert.Equal(room, newRoom);
      Assert.True(newRoom?.Id == id);
    }

    private async Task<FrontDesk.Core.Aggregates.Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Room, int>(room);

      return room;
    }
  }
}
