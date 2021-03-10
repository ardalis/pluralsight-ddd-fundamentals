using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.AppointmentTypeTests
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository<AppointmentType> _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository<AppointmentType>();
    }

    [Fact]
    public async Task ListsAppointmentTypeAfterAddingIt()
    {
      await AddAppointmentType();

      var clients = await _repository.ListAsync();

      Assert.True(clients?.Count > 0);
    }

    private async Task<AppointmentType> AddAppointmentType()
    {
      var client = new AppointmentTypeBuilder().Id(7).Build();

      await _repository.AddAsync(client);

      return client;
    }
  }
}
