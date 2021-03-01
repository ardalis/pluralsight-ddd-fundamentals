using System;
using FrontDesk.Core.Aggregates;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Create
  {
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime;
    private readonly Guid _scheduleId = Guid.NewGuid();
    private readonly int _testClientId = 1;
    private readonly int _testPatientId = 2;
    private readonly int _testRoomId = 3;
    private readonly int _testAppointmentTypeId = 4;
    private readonly int _testDoctorId = 5;
    private readonly string _testTitle = "Test Title";

    public Appointment_Create()
    {
      _endTime = _startTime.AddHours(3);
    }

    [Fact]
    public void CreateSuccess()
    {
      var appointment = Appointment.Create(_scheduleId, _testClientId, _testPatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);
      const int threeHours = 3 * 60;

      Assert.Null(appointment.DateTimeConfirmed);
      Assert.Equal(_scheduleId, appointment.ScheduleId);
      Assert.Equal(_testClientId, appointment.ClientId);
      Assert.Equal(_testPatientId, appointment.PatientId);
      Assert.Equal(_testRoomId, appointment.RoomId);
      Assert.Equal(_testAppointmentTypeId, appointment.AppointmentTypeId);
      Assert.Equal(_testDoctorId, appointment.DoctorId);
      Assert.Equal(threeHours, appointment.TimeRange.DurationInMinutes());
      Assert.Equal(_testTitle, appointment.Title);
    }

    [Fact]
    public void ThrowExceptionZeroClientId()
    {
      const int zeroClientId = 0;

      void Action() => Appointment.Create(_scheduleId, zeroClientId, _testPatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionNegativeClientId()
    {
      const int negativeClientId = -1;

      void Action() => Appointment.Create(_scheduleId, negativeClientId, _testPatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionZeroPatientId()
    {
      const int zeroPatientId = 0;

      void Action() => Appointment.Create(_scheduleId, _testClientId, zeroPatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionNegativePatientId()
    {
      const int negativePatientId = -1;

      void Action() => Appointment.Create(_scheduleId, _testClientId, negativePatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionZeroRoomId()
    {
      const int zeroRoomId = 0;

      void Action() => Appointment.Create(_scheduleId, _testClientId, _testPatientId, zeroRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionNegativeRoomId()
    {
      const int negativeRoomId = -1;

      void Action() => Appointment.Create(_scheduleId, _testClientId, _testPatientId, negativeRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionZeroAppointmentTypeId()
    {
      const int zeroAppointmentTypeId = 0;

      void Action() => Appointment.Create(_scheduleId, _testClientId, _testPatientId, _testRoomId, _startTime, _endTime, zeroAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionNegativeAppointmentTypeId()
    {
      const int negativeAppointmentTypeId = -1;

      void Action() => Appointment.Create(_scheduleId, _testClientId, _testPatientId, _testRoomId, _startTime, _endTime, negativeAppointmentTypeId, _testDoctorId, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Fact]
    public void ThrowExceptionNullTitle()
    {
      const string nullTitle = null;

      void Action() => Appointment.Create(_scheduleId, _testClientId, _testPatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, nullTitle);

      Assert.Throws<ArgumentNullException>(Action);
    }

    [Fact]
    public void ThrowExceptionEmptyTitle()
    {
      const string emptyTitle = "";

      void Action() => Appointment.Create(_scheduleId, _testClientId, _testPatientId, _testRoomId, _startTime, _endTime, _testAppointmentTypeId, _testDoctorId, emptyTitle);

      Assert.Throws<ArgumentException>(Action);
    }
  }
}
