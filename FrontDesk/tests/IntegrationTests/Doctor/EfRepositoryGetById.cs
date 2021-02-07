using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task GetsByIdDoctorAfterAddingIt()
    {
      var id = 9;
      var doctor = await AddDoctor(id);

      var newDoctor = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Doctor, int>(id);

      Assert.Equal(doctor, newDoctor);
      Assert.True(newDoctor?.Id == id);
    }

    private async Task<FrontDesk.Core.Aggregates.Doctor> AddDoctor(int id)
    {
      var doctor = new DoctorBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
