using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.AppointmentTypeTests
{
  public class EfRepositoryList : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryList(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ListsAppointmentTypeAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var appointmentType = new AppointmentTypeBuilder().Id(0).Build();

        var repo1 = new EfRepository<AppointmentType>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(appointmentType);

        var repo2 = new EfRepository<AppointmentType>(Fixture.CreateContext(transaction));
        var appointmentTypes = (await repo2.ListAsync()).ToList();

        Assert.True(appointmentTypes?.Count > 0);
      }
    }
  }
}
