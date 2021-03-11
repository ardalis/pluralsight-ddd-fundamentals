using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Doctor> _repository;

    public EfRepositoryUpdate()
    {
      _repository = GetRepository<Doctor>();
    }

    [Fact]
    public async Task UpdatesDoctorAfterAddingIt()
    {
      var id = 2;
      var name = "changed";

      var doctor = await AddDoctor(id);

      doctor.Name = name;
      await _repository.UpdateAsync(doctor);

      var updatedDoctor = await _repository.GetByIdAsync(id);

      Assert.Equal(name, updatedDoctor.Name);
    }

    private async Task<Doctor> AddDoctor(int id)
    {
      var doctor = new DoctorBuilder().Id(id).Build();

      await _repository.AddAsync(doctor);

      return doctor;
    }
  }
}
