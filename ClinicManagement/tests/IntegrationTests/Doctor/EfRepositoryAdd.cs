using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryAdd()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task AddsDoctorAndSetsId()
    {
      var doctor = await AddDoctor();

      var newDoctor = (await _repository.ListAsync<ClinicManagement.Core.Aggregates.Doctor, int>()).FirstOrDefault();

      Assert.Equal(doctor, newDoctor);
      Assert.True(newDoctor?.Id > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(2).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
