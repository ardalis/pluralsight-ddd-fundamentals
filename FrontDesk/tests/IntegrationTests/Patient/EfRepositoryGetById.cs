using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Client
{
  public class EfRepositoryGetById : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryGetById()
    {
      _repository = GetRepository();
    }

    //[Fact]
    //public async Task GetsByIdPatientAfterAddingIt()
    //{
    //  var id = 9;
    //  var patient = await AddPatient(id);

    //  var newPatient = await _repository.GetByIdAsync<FrontDesk.Core.Aggregates.Patient, int>(id);

    //  Assert.Equal(patient, newPatient);
    //  Assert.True(newPatient?.Id == id);
    //}

    //private async Task<FrontDesk.Core.Aggregates.Patient> AddPatient(int id)
    //{
    //  var patient = new PatientBuilder().Id(id).Build();

    //  await _repository.AddAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

    //  return patient;
    //}
  }
}
