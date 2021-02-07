using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryDelete : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryDelete()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task DeletesDoctorAfterAddingIt()
    {
      var id = 8;

      var doctor = await AddDoctor(id);
      await _repository.DeleteAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      Assert.DoesNotContain(await _repository.ListAsync<FrontDesk.Core.Aggregates.Doctor, int>(),
          i => i.Id == id);
    }

    private async Task<FrontDesk.Core.Aggregates.Doctor> AddDoctor(int id)
    {
      var doctor = new DoctorBuilder().Id(id).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
