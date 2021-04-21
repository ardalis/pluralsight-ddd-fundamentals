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
  public class EfRepositoryGetByScheduleForClinicAndDateWithAppointmentsSpec : IClassFixture<SharedDatabaseFixture>
  {
    public SharedDatabaseFixture Fixture { get; }
    public EfRepositoryGetByScheduleForClinicAndDateWithAppointmentsSpec(SharedDatabaseFixture fixture) => Fixture = fixture;

    [Fact]
    public async Task ReturnScheduleWithAllAppointments()
    {
      using (var transaction = await Fixture.Connection.BeginTransactionAsync())
      {
        var id = Guid.NewGuid();
        var newSchedule = new ScheduleBuilder().WithDefaultValues().WithId(id).Build();

        // add 4 appointments on 3 different days
        var appointment1 = new AppointmentBuilder().WithDefaultValues().Build();
        newSchedule.AddNewAppointment(appointment1);
        newSchedule.AddNewAppointment(new AppointmentBuilder()
          .WithDefaultValues()
          .WithDateTimeOffsetRange(new DateTimeOffsetRange(appointment1.TimeRange.Start.AddDays(3), TimeSpan.FromHours(1)))
          .Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder()
          .WithDefaultValues()
          .WithDateTimeOffsetRange(new DateTimeOffsetRange(appointment1.TimeRange.Start.AddDays(5), TimeSpan.FromHours(1)))
          .Build());
        newSchedule.AddNewAppointment(new AppointmentBuilder().WithDefaultValues().Build());

        var repo1 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        await repo1.AddAsync(newSchedule);

        var spec = new ScheduleForClinicAndDateWithAppointmentsSpec(ScheduleBuilder.TEST_CLINIC_ID, new DateTimeOffset(AppointmentBuilder.TEST_START_TIME.Date, TimeSpan.FromHours(-4)));
        var repo2 = new EfRepository<Schedule>(Fixture.CreateContext(transaction));
        var scheduleFromRepo = await repo2.GetBySpecAsync(spec);

        Assert.True(scheduleFromRepo.Id == id);
        Assert.Equal(2, scheduleFromRepo.Appointments.Count()); // only returns the first and last appointment added
      }
    }
  }
}
