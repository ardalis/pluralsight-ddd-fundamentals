using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
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
      await _repository.UpdateAsync<ClinicManagement.Core.Aggregates.Room, int>(room);

      var updatedRoom = await _repository.GetByIdAsync<ClinicManagement.Core.Aggregates.Room, int>(id);

      Assert.Equal(name, updatedRoom.Name);
    }

    private async Task<ClinicManagement.Core.Aggregates.Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Room, int>(room);

      return room;
    }
  }
}
