using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.Client
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository();
    }

    //[Fact]
    //public async Task ListsPatientAfterAddingIt()
    //{
    //  await AddPatient();

    //  var patients = (await _repository.ListAsync<FrontDesk.Core.Aggregates.Patient, int>()).ToList();

    //  Assert.True(patients?.Count > 0);
    //}

    //private async Task<FrontDesk.Core.Aggregates.Patient> AddPatient()
    //{
    //  var patient = new PatientBuilder().Id(7).Build();

    //  await _repository.AddAsync<FrontDesk.Core.Aggregates.Patient, int>(patient);

    //  return patient;
    //}
  }
}
