using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Client> _repository;

    public EfRepositoryAdd()
    {
      _repository = GetRepository<Client>();
    }

    [Fact]
    public async Task AddsClientAndSetsId()
    {
      var client = await AddClient();

      var newClient = (await _repository.ListAsync()).FirstOrDefault();

      Assert.Equal(client, newClient);
      Assert.True(newClient?.Id > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.Client> AddClient()
    {
      var client = new ClientBuilder().Id(2).Build();

      await _repository.AddAsync(client);

      return client;
    }
  }
}
