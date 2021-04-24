using System;
using System.Linq;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_DeleteAppointment
  {
    private readonly Guid _scheduleId = Guid.Parse("4a17e702-c20e-4b87-b95b-f915c5a794f7");
    private readonly DateTimeOffsetRange _dateRange = new DateTimeOffsetRange(DateTime.Today, DateTime.Today.AddDays(1));
    private readonly int _clinicId = 1;

    [Fact]
    public void RemovesAppointment()
    {
      var schedule = CreateScheduleWithAppointment();

      schedule.DeleteAppointment(schedule.Appointments.First());

      Assert.False(schedule.Appointments.Any());
    }

    private Schedule CreateScheduleWithAppointment()
    {
      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId);
      var appointmentType = 1;
      var doctorId = 2;
      var patientId = 3;
      var roomId = 4;

      DateTime lisaStartTime = new DateTime(2021, 01, 01, 10, 00, 00);
      DateTime lisaEndTime = new DateTime(2021, 01, 01, 11, 00, 00);
      var lisaDateRange = new DateTimeOffsetRange(lisaStartTime, lisaEndTime);
      var lisaTitle = "Lisa Appointment";
      var lisaAppointment = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId,
        lisaDateRange, lisaTitle);
      schedule.AddNewAppointment(lisaAppointment);

      return schedule;
    }
  }
}
