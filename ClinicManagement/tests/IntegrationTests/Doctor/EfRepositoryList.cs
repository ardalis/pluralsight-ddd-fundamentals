using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
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

      var doctors = (await _repository.ListAsync<ClinicManagement.Core.Aggregates.Doctor, int>()).ToList();

      Assert.True(doctors?.Count > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(7).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
