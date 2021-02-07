using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Patient
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryAdd()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task AddsPatientAndSetsId()
    {
      var patient = await AddPatient();

      var newPatient = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Patient, int>()).FirstOrDefault();

      Assert.Equal(patient, newPatient);
      Assert.True(newPatient?.Id > 0);
    }

    private async Task<FrontDesk.Core.Aggregates.Patient> AddPatient()
    {
      var patient = new PatientBuilder().Id(2).Build();

      await _repository.AddAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

      return patient;
    }
  }
}
