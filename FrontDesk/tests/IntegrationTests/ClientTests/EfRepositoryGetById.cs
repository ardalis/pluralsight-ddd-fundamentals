using System.Threading.Tasks;
using FrontDesk.Core.Aggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Client> _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepositoryAsync<Client>().Result;
    }

    [Fact]
    public async Task GetsByIdClientAfterAddingIt()
    {
      var id = 100;
      var client = await AddClient(id);

      var newClient = await _repository.GetByIdAsync(id);

      Assert.Equal(client, newClient);
      Assert.True(newClient?.Id == id);
    }

    private async Task<Client> AddClient(int id)
    {
      var client = new ClientBuilder().Id(id).Build();

      await _repository.AddAsync(client);

      return client;
    }
  }
}
