using System.Threading.Tasks;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryDelete : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Doctor> _repository;

    public EfRepositoryDelete()
    {
      _repository = GetRepository<Doctor>();
    }

    [Fact]
    public async Task DeletesDoctorAfterAddingIt()
    {
      var id = 8;

      var doctor = await AddDoctor(id);
      await _repository.DeleteAsync(doctor);

      Assert.DoesNotContain(await _repository.ListAsync(),
          i => i.Id == id);
    }

    private async Task<Doctor> AddDoctor(int id)
    {
      var doctor = new DoctorBuilder().Id(id).Build();

      await _repository.AddAsync(doctor);

      return doctor;
    }
  }
}
