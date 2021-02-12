using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Api;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Doctor
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryAdd(CustomWebApplicationFactory<Startup> factory) : base(factory)
    {
      _repository = GetRepositoryAsync().Result;
    }

    [Fact]
    public async Task AddsDoctorAndSetsId()
    {
      var doctor = await AddDoctor();

      var newDoctor = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Doctor, int>()).FirstOrDefault();

      Assert.Equal(doctor, newDoctor);
      Assert.True(newDoctor?.Id > 0);
    }

    private async Task<FrontDesk.Core.Aggregates.Doctor> AddDoctor()
    {
      var doctor = new DoctorBuilder().Id(2).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Doctor, int>(doctor);

      return doctor;
    }
  }
}
