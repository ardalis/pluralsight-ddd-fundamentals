using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task ListsClientAfterAddingIt()
    {
      await AddClient();

      var clients = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Client, int>()).ToList();

      Assert.True(clients?.Count > 0);
    }

    private async Task<FrontDesk.Core.Aggregates.Client> AddClient()
    {
      var client = new ClientBuilder().Id(7).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
