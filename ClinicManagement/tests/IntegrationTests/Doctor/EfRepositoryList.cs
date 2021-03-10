using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Doctor> _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository<Doctor>();
    }

    [Fact]
    public async Task ListsDoctorAfterAddingIt()
    {
      await AddDoctor();

      var doctors = await _repository.ListAsync();

      Assert.True(doctors?.Count > 0);
    }

    private async Task<Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(7).Build();

      await _repository.AddAsync(doctor);

      return doctor;
    }
  }
}
