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
  public class EfRepositoryGetByScheduleByIdWithAppointmentsSpec : BaseEfRepoTestFixture
  {
    private readonly EfRepository<Schedule> _repository;

    public EfRepositoryGetByScheduleByIdWithAppointmentsSpec()
    {
      _repository = GetRepositoryAsync<Schedule>().Result;
    }

    [Fact]
    public async Task ReturnScheduleWithAllAppointments()
    {
      var id = Guid.NewGuid();
      var newSchedule = await AddSchedule(id);

      var builder = new AppointmentBuilder();
      newSchedule.AddNewAppointment(builder.WithDefaultValues().Build());
      newSchedule.AddNewAppointment(builder.WithDefaultValues().Build());
      newSchedule.AddNewAppointment(builder.WithDefaultValues().Build());

      var spec = new ScheduleByIdWithAppointmentsSpec(id);
      var scheduleFromRepo = await _repository.GetBySpecAsync(spec);

      Assert.Equal(newSchedule, scheduleFromRepo);
      Assert.True(scheduleFromRepo.Id == id);
      Assert.Equal(3, scheduleFromRepo.Appointments.Count());
    }

    private async Task<Schedule> AddSchedule(Guid id)
    {
      int clinicId = 2;
      var schedule = new Schedule(id, new DateTimeOffsetRange(DateTimeOffset.Now.Date, TimeSpan.FromDays(1)), clinicId);

      await _repository.AddAsync(schedule);

      return schedule;
    }
  }
}
