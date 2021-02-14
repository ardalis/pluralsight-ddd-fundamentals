using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryUpdate : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;
    private int _testDoctorId = 567;

    public EfRepositoryUpdate()
    {
      _repository = GetRepositoryAsync().Result;
    }

    [Fact]
    public async Task UpdatesDoctorAfterAddingIt()
    {
      var name = "changed";

      var doctor = await AddDoctor(_testDoctorId);

      doctor.UpdateName(name);
      await _repository.UpdateAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      var updatedDoctor = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Doctor, int>(_testDoctorId);

      Assert.Equal(name, updatedDoctor.Name);
    }

    private async Task<FrontDesk.Core.Aggregates.Doctor> AddDoctor(int id)
    {
      var doctor = new DoctorBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
