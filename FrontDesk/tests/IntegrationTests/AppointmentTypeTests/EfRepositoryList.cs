using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.AppointmentTypeTests
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository<AppointmentType> _repository;

    public EfRepositoryList()
    {
      _repository = GetRepositoryAsync<AppointmentType>().Result;
    }

    [Fact]
    public async Task ListsAppointmentTypeAfterAddingIt()
    {
      await AddAppointmentType();

      var appointmentTypes = (await _repository.ListAsync()).ToList();

      Assert.True(appointmentTypes?.Count > 0);
    }

    private async Task<AppointmentType> AddAppointmentType()
    {
      var appointmentType = new AppointmentTypeBuilder().Id(7).Build();

      await _repository.AddAsync(appointmentType);

      return appointmentType;
    }
  }
}
