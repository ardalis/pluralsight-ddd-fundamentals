using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task ListsDoctorAfterAddingIt()
    {
      await AddDoctor();

      var doctors = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Doctor, int>()).ToList();

      Assert.True(doctors?.Count > 0);
    }

    private async Task<FrontDesk.Core.Aggregates.Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(7).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
