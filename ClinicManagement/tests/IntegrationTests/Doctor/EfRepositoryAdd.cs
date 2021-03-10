using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Doctor> _repository;

    public EfRepositoryAdd()
    {
      _repository = GetRepository<Doctor>();
    }

    [Fact]
    public async Task AddsDoctorAndSetsId()
    {
      var doctor = await AddDoctor();

      var newDoctor = (await _repository.ListAsync()).FirstOrDefault();

      Assert.Equal(doctor, newDoctor);
      Assert.True(newDoctor?.Id > 0);
    }

    private async Task<Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(2).Build();

      await _repository.AddAsync(doctor);

      return doctor;
    }
  }
}
