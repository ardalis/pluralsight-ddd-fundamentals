using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryUpdate()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task UpdatesClientAfterAddingIt()
    {
      var id = 2;
      var fullName = "changed";

      var client = await AddClient(id);

      client.UpdateFullName(fullName);
      await _repository.UpdateAsync<ClinicManagement.Core.Aggregates.Client, int>(client);

      var updatedClient = await _repository.GetByIdAsync<ClinicManagement.Core.Aggregates.Client, int>(id);

      Assert.Equal(fullName, updatedClient.FullName);
    }

    private async Task<ClinicManagement.Core.Aggregates.Client> AddClient(int id)
    {
      var client = new ClientBuilder().Id(id).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
