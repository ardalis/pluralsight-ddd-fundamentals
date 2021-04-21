using System;
using System.Linq;
using System.Threading.Tasks;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using FrontDesk.Infrastructure.Data;
using PluralsightDdd.SharedKernel;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ScheduleTests
{
  public class EfRepositoryGetByScheduleByIdWithAppointmentsSpec : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetByScheduleByIdWithAppointmentsSpec(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ReturnScheduleWithAllAppointments()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var id = Guid.NewGuid();
        var newSchedule = new ScheduleBuilder().WithDefaultValues().WithId(id).Build();

        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());

        var repo1 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(newSchedule);

        var spec = new ScheduleByIdWithAppointmentsSpec(id);
        var repo2 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        var scheduleFromRepo = await repo2.GetBySpecAsync(spec);

        Assert.Equal(newSchedule.Id, scheduleFromRepo.Id);
        Assert.True(scheduleFromRepo.Id == id);
        Assert.Equal(3, scheduleFromRepo.Appointments.Count());
      }
    }
  }
}
