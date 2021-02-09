using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task GetsByIdClientAfterAddingIt()
    {
      var id = 9;
      var client = await AddClient(id);

      var newClient = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Client, int>(id);

      Assert.Equal(client, newClient);
      Assert.True(newClient?.Id == id);
    }

    private async Task<FrontDesk.Core.Aggregates.Client> AddClient(int id)
    {
      var client = new ClientBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
