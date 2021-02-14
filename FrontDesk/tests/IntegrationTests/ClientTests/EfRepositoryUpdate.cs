using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;
    private int _testClientId = 345;

    public EfRepositoryUpdate()
    {
      _repository = GetRepositoryAsync().Result;
    }

    [Fact]
    public async Task UpdatesClientAfterAddingIt()
    {
      var fullName = "changed";

      var client = await AddClient(_testClientId);

      client.UpdateFullName(fullName);
      await _repository.UpdateAsync<FrontDesk.Core.Aggregates.Client, int>(client);

      var updatedClient = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Client, int>(_testClientId);

      Assert.Equal(fullName, updatedClient.FullName);
    }

    private async Task<FrontDesk.Core.Aggregates.Client> AddClient(int id)
    {
      var client = new ClientBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
