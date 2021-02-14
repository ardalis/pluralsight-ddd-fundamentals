using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_AddNewAppointment
  {
    private readonly Guid _scheduleId = Guid.Parse("4a17e702-c20e-4b87-b95b-f915c5a794f7");
    private readonly DateTimeRange _dateRange;
    private readonly int _clinicId = 1;

    [Fact]
    public async Task ThrowsGivenDuplicateAppointment()
    {

    }
    [Fact]
    public async Task MarksConflictingAppointments()
    {

    }
    [Fact]
    public void AddsAppointmentScheduledEvent()
    {
      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId, null);
      var appointmentType = 1;
      var doctorId = 2;
      var patientId = 3;
      var roomId = 4;

      DateTime lisaStartTime = new DateTime(2021, 01, 01, 10, 00, 00);
      DateTime lisaEndTime = new DateTime(2021, 01, 01, 11, 00, 00);
      var lisaDateRange = new DateTimeRange(lisaStartTime, lisaEndTime);
      var lisaTitle = "Lisa Appointment";
      var lisaAppointment = new Appointment(appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, lisaDateRange, lisaTitle);
      schedule.AddNewAppointment(lisaAppointment);

      DateTime mimiStartTime = new DateTime(2021, 01, 01, 12, 00, 00);
      DateTime mimiEndTime = new DateTime(2021, 01, 01, 14, 00, 00);
      var mimiDateRange = new DateTimeRange(mimiStartTime, mimiEndTime);
      var mimiTitle = "Mimi Appointment";
      var mimiAppointment = new Appointment(appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, mimiDateRange, mimiTitle);
      schedule.AddNewAppointment(mimiAppointment);

      Assert.Equal(2, schedule.Appointments.Count());
      Assert.False(lisaAppointment.IsPotentiallyConflicting);
      Assert.False(mimiAppointment.IsPotentiallyConflicting);
    }

  }
}
