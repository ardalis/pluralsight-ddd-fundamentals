using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.DoctorTests
{
  public class EfRepositoryList : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryList(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ListsDoctorAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var doctor = new DoctorBuilder().WithDefaultValues().Build();

        var repo1 = new EfRepository<Doctor>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(doctor);

        var repo2 = new EfRepository<Doctor>(Fixture.CreateContext(transaction));
        var doctors = (await repo2.ListAsync()).ToList();

        Assert.True(doctors?.Count > 0);
      }
    }
  }
}
