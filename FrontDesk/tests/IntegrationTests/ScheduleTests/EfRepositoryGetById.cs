using System;
using System.Threading.Tasks;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Infrastructure.Data;
using PluralsightDdd.SharedKernel;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ScheduleTests
{
  public class EfRepositoryGetById : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetById(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task GetsByIdScheduleAfterAddingIt()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {

        var id = Guid.NewGuid();
        int clinicId = 2;
        var newSchedule = new Schedule(id, new DateTimeOffsetRange(DateTimeOffset.Now.Date, TimeSpan.FromDays(1)), clinicId);

        var builder = new AppointmentBuilder();
        newSchedule.AddNewAppointment(builder.WithDefaultValues().Build());
        newSchedule.AddNewAppointment(builder.WithDefaultValues().Build());
        newSchedule.AddNewAppointment(builder.WithDefaultValues().Build());

        var repo1 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(newSchedule);


        var repo2 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));

        var scheduleFromRepo = await repo2.GetByIdAsync(id);

        Assert.Equal(newSchedule.Id, scheduleFromRepo.Id);
        Assert.True(scheduleFromRepo.Id == id);
        Assert.Empty(scheduleFromRepo.Appointments);
      }
    }
  }
}
