using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
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
      await _repository.DeleteAsync<ClinicManagement.Core.Aggregates.Room, int>(room);

      Assert.DoesNotContain(await _repository.ListAsync<ClinicManagement.Core.Aggregates.Room, int>(),
          i => i.Id == id);
    }

    private async Task<ClinicManagement.Core.Aggregates.Room> AddRoom(int id)
    {
      var room = new RoomBuilder().Id(id).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Room, int>(room);

      return room;
    }
  }
}
