using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Client
{
  public class EfRepositoryDelete : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryDelete()
    {
      _repository = GetRepository();
    }

    //[Fact]
    //public async Task DeletesPatientAfterAddingIt()
    //{
    //  var id = 8;

    //  var patient = await AddPatient(id);
    //  await _repository.DeleteAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

    //  Assert.DoesNotContain(await _repository.ListAsync<FrontDesk.Core.Aggregates.Patient, int>(),
    //      i => i.Id == id);
    //}

    //private async Task<FrontDesk.Core.Aggregates.Patient> AddPatient(int id)
    //{
    //  var patient = new PatientBuilder().Id(id).Build();

    //  await _repository.AddAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

    //  return patient;
    //}
  }
}
