using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.RoomTests
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Room> _repository;

    public EfRepositoryAdd()
    {
      _repository = GetRepository<Room>();
    }

    [Fact]
    public async Task AddsRoomAndSetsId()
    {
      var room = await AddRoom();

      var newRoom = (await _repository.ListAsync()).FirstOrDefault();

      Assert.Equal(room, newRoom);
      Assert.True(newRoom?.Id > 0);
    }

    private async Task<Room> AddRoom()
    {
      var room = new RoomBuilder().Id(2).Build();

      await _repository.AddAsync(room);

      return room;
    }
  }
}
