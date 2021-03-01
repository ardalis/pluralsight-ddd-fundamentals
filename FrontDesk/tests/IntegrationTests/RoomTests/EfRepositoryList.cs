using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.Aggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.RoomTests
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Room> _repository;

    public EfRepositoryList()
    {
      _repository = GetRepositoryAsync<Room>().Result;
    }

    [Fact]
    public async Task ListsRoomAfterAddingIt()
    {
      await AddRoom();

      var rooms = (await _repository.ListAsync()).ToList();

      Assert.True(rooms?.Count > 0);
    }

    private async Task<FrontDesk.Core.Aggregates.Room> AddRoom()
    {
      var room = new RoomBuilder().Id(7).Build();

      await _repository.AddAsync(room);

      return room;
    }
  }
}
