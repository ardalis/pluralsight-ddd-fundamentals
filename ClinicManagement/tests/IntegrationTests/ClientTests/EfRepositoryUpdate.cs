using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Client> _repository;

    public EfRepositoryUpdate()
    {
      _repository = GetRepository<Client>();
    }

    [Fact]
    public async Task UpdatesClientAfterAddingIt()
    {
      var id = 2;
      var fullName = "changed";

      var client = await AddClient(id);

      client.FullName = fullName;
      await _repository.UpdateAsync(client);

      var updatedClient = await _repository.GetByIdAsync(id);

      Assert.Equal(fullName, updatedClient.FullName);
    }

    private async Task<Client> AddClient(int id)
    {
      var client = new ClientBuilder().Id(id).Build();

      await _repository.AddAsync(client);

      return client;
    }
  }
}
