using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;
using UnitTests.Builders;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_MarkConflictingAppointments
  {
    private const int CLINIC_ID = 1;
    private const int APPOINTMENT_TYPE_ID = 2;
    private const int CLIENT_ID = 3;
    private const int DOCTOR_ID = 4;
    private const int PATIENT_ID = 5;
    private const int ROOM_ID = 6;

    private readonly Guid _scheduleId = Guid.Parse("4a17e702-c20e-4b87-b95b-f915c5a794f7");
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime = new DateTime(2021, 01, 01, 11, 00, 00);
    private readonly DateTimeOffsetRange _dateRange;

    public Schedule_MarkConflictingAppointments()
    {
      _dateRange = new DateTimeOffsetRange(_startTime, _endTime);
    }

    [Fact]
    public void SetsProperties()
    {
      var schedule = CreateSchedule();

      Assert.Equal(_scheduleId, schedule.Id);
      Assert.Equal(_startTime, schedule.DateRange.Start);
      Assert.Equal(_endTime, schedule.DateRange.End);
      Assert.Equal(CLINIC_ID, schedule.ClinicId);
    }

    [Fact]
    public void MarksConflictingAppointmentsDueToSamePatientId()
    {
      // Arrange
      var schedule = CreateSchedule();
      var appointment1 = CreateAppointment(APPOINTMENT_TYPE_ID,     CLIENT_ID,     DOCTOR_ID,     PATIENT_ID, ROOM_ID);
      var appointment2 = CreateAppointment(APPOINTMENT_TYPE_ID + 1, CLIENT_ID + 1, DOCTOR_ID + 1, PATIENT_ID, ROOM_ID + 1);

      // Act
      schedule.AddNewAppointment(appointment1);
      schedule.AddNewAppointment(appointment2);

      // Assert
      Assert.True(appointment1.IsPotentiallyConflicting);
      Assert.True(appointment2.IsPotentiallyConflicting);
    }

    [Fact]
    public void MarksConflictingAppointmentsForSameAnimalInTwoRoomsAtSameTime()
    {
      var schedule = CreateSchedule();
      var appointmentType = 1;
      var doctorId = 2;
      var patientId = 3;
      var roomId = 4;

      var lisaTitle = "Lisa Appointment";
      var lisaAppointment = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, CLIENT_ID, doctorId, patientId, roomId, _dateRange, lisaTitle);
      schedule.AddNewAppointment(lisaAppointment);

      var lisaTitle2 = "Lisa Appointment 2";
      var lisaAppointment2 = new Appointment(Guid.NewGuid(), appointmentType, _scheduleId, CLIENT_ID, doctorId, patientId, roomId+1, _dateRange, lisaTitle2);
      schedule.AddNewAppointment(lisaAppointment2);

      Assert.True(lisaAppointment.IsPotentiallyConflicting, "lisa1 not conflicting");
      Assert.True(lisaAppointment2.IsPotentiallyConflicting, "lisa2 not conflicting");
    }

    private Appointment CreateAppointment(int appointmentTypeId, int clientId, int doctorId, int patientId, int roomId)
    {
      var appointmentBuilder = new AppointmentBuilder();

      return appointmentBuilder.
        WithDefaultValues().
        WithDateTimeOffsetRange(_dateRange).
        WithAppointmentTypeId(appointmentTypeId).
        WithClientId(clientId).
        WithDoctorId(doctorId).
        WithPatientId(patientId).
        WithRoomId(roomId).
        Build();
    }

    private Schedule CreateSchedule()
    {
      return new Schedule(_scheduleId, _dateRange, CLINIC_ID);
    }
  }
}
