using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;
    private int _testDoctorId = 567;

    public EfRepositoryAdd()
    {
      _repository = GetRepositoryAsync().Result;
    }

    [Fact]
    public async Task AddsDoctorAndSetsId()
    {
      var doctor = await AddDoctor();

      var newDoctor = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Doctor, int>()).FirstOrDefault(d => d.Id == _testDoctorId);

      Assert.Equal(doctor, newDoctor);
      Assert.True(newDoctor?.Id > 0);
    }

    private async Task<FrontDesk.Core.Aggregates.Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(_testDoctorId).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
