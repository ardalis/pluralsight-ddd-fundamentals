using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.RoomTests
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Room> _repository;

    public EfRepositoryUpdate()
    {
      _repository = GetRepository<Room>();
    }

    [Fact]
    public async Task UpdatesRoomAfterAddingIt()
    {
      var id = 2;
      var name = "changed";

      var room = await AddRoom(id);

      room.Name = name;
      await _repository.UpdateAsync(room);

      var updatedRoom = await _repository.GetByIdAsync(id);

      Assert.Equal(name, updatedRoom.Name);
    }

    private async Task<Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync(room);

      return room;
    }
  }
}
