using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_MarkConflictingAppointments
  {
    private readonly Guid _scheduleId = Guid.Parse("4a17e702-c20e-4b87-b95b-f915c5a794f7");
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime = new DateTime(2021, 01, 01, 11, 00, 00);
    private readonly DateTimeOffsetRange _dateRange;
    private readonly int _clinicId = 1;

    public Schedule_MarkConflictingAppointments()
    {
      _dateRange = new DateTimeOffsetRange(_startTime, _endTime);
    }

    [Fact]
    public void SetsProperties()
    {
      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId);

      Assert.Equal(_scheduleId, schedule.Id);
      Assert.Equal(_startTime, schedule.DateRange.Start);
      Assert.Equal(_endTime, schedule.DateRange.End);
      Assert.Equal(_clinicId, schedule.ClinicId);
    }

    [Fact]
    public void MarksConflictingAppointments()
    {
      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId);
      var appointmentType = 1;
      var doctorId = 2;
      var patientId = 3;
      var roomId = 4;

      var lisaTitle = "Lisa Appointment";
      var lisaAppointment = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, _dateRange, lisaTitle);
      schedule.AddNewAppointment(lisaAppointment);

      var mimiTitle = "Mimi Appointment";
      var mimiAppointment = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, _dateRange, mimiTitle);
      schedule.AddNewAppointment(mimiAppointment);

      Assert.True(lisaAppointment.IsPotentiallyConflicting, "lisa not conflicting");
      Assert.True(mimiAppointment.IsPotentiallyConflicting, "mimi not conflicting");
    }

    [Fact]
    public void MarksConflictingAppointmentsForSameAnimalInTwoRoomsAtSameTime()
    {
      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId);
      var appointmentType = 1;
      var doctorId = 2;
      var patientId = 3;
      var roomId = 4;

      var lisaTitle = "Lisa Appointment";
      var lisaAppointment = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId, _dateRange, lisaTitle);
      schedule.AddNewAppointment(lisaAppointment);

      var lisaTitle2 = "Lisa Appointment 2";
      var lisaAppointment2 = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, _clinicId, doctorId, patientId, roomId+1, _dateRange, lisaTitle2);
      schedule.AddNewAppointment(lisaAppointment2);

      Assert.True(lisaAppointment.IsPotentiallyConflicting, "lisa1 not conflicting");
      Assert.True(lisaAppointment2.IsPotentiallyConflicting, "lisa2 not conflicting");
    }
  }
}
