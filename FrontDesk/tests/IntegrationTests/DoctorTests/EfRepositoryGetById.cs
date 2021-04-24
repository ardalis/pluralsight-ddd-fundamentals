using System;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryGetById : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetById(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task GetsByIdDoctorAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        string name = Guid.NewGuid().ToString();
        var doctor = new DoctorBuilder().WithName(name).Build();

        var repo1 = new EfRepository<Doctor>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(doctor);

        var repo2 = new EfRepository<Doctor>(Fixture.CreateContext(transaction));
        var doctorFromDb = (await repo2.GetByIdAsync(doctor.Id));

        Assert.Equal(doctor.Id, doctorFromDb.Id);
        Assert.Equal(doctor.Name, doctorFromDb.Name);
      }
    }
  }
}
