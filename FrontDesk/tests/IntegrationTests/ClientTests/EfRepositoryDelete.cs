using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryDelete : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryDelete()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task DeletesClientAfterAddingIt()
    {
      var id = 8;

      var client = await AddClient(id);
      await _repository.DeleteAsync<FrontDesk.Core.Aggregates.Client, int>(client);

      Assert.DoesNotContain(await _repository.ListAsync<FrontDesk.Core.Aggregates.Client, int>(),
          i => i.Id == id);
    }

    private async Task<FrontDesk.Core.Aggregates.Client> AddClient(int id)
    {
      var client = new ClientBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
