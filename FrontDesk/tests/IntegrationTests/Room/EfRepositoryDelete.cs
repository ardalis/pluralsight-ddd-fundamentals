using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Room
{
  public class EfRepositoryDelete : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryDelete()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task DeletesRoomAfterAddingIt()
    {
      var id = 8;

      var room = await AddRoom(id);
      await _repository.DeleteAsync<FrontDesk.Core.Aggregates.Room, int>(room);

      Assert.DoesNotContain(await _repository.ListAsync<FrontDesk.Core.Aggregates.Room, int>(),
          i => i.Id == id);
    }

    private async Task<FrontDesk.Core.Aggregates.Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Room, int>(room);

      return room;
    }
  }
}
