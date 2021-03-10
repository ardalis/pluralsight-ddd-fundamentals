using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.RoomTests
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Room> _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepository<Room>();
    }

    [Fact]
    public async Task GetsByIdRoomAfterAddingIt()
    {
      var id = 9;
      var room = await AddRoom(id);

      var newRoom = await _repository.GetByIdAsync(id);

      Assert.Equal(room, newRoom);
      Assert.True(newRoom?.Id == id);
    }

    private async Task<Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync(room);

      return room;
    }
  }
}
