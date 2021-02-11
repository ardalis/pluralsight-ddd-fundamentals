using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Room
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task ListsRoomAfterAddingIt()
    {
      await AddRoom();

      var rooms = (await _repository.ListAsync<ClinicManagement.Core.Aggregates.Room, int>()).ToList();

      Assert.True(rooms?.Count > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.Room> AddRoom()
    {
      var room = new RoomBuilder().Id(7).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Room, int>(room);

      return room;
    }
  }
}
