using System;
using FrontDesk.Core.Aggregates;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Create
  {
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly Guid _scheduleId = Guid.NewGuid();

    [Fact]
    public void CreateSuccess()
    {
      var appointment = Appointment.Create(_scheduleId, 1, 2, 3, _startTime, _startTime.AddHours(3), 4, 5, "Title Test");

      Assert.Null(appointment.DateTimeConfirmed);
      Assert.Equal(_scheduleId, appointment.ScheduleId);
      Assert.Equal(1, appointment.ClientId);
      Assert.Equal(2, appointment.PatientId);
      Assert.Equal(3, appointment.RoomId);
      Assert.Equal(4, appointment.AppointmentTypeId);
      Assert.Equal(5, appointment.DoctorId);
      Assert.Equal(3 * 60, appointment.TimeRange.DurationInMinutes());
      Assert.Equal("Title Test", appointment.Title);
    }

    [Fact]
    public void ThrowExceptionZeroClientId()
    {
      const int zeroClientId = 0;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, zeroClientId, 1, 2, _startTime, _startTime.AddHours(3), 3, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativeClientId()
    {
      const int negativeClientId = -1;

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, negativeClientId, 1, 2, _startTime, _startTime.AddHours(3), 3, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionZeroPatientId()
    {
      const int zeroPatientId = 0;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, zeroPatientId, 2, _startTime, _startTime.AddHours(3), 3, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativePatientId()
    {
      const int negativePatientId = -1;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, negativePatientId, 1, _startTime, _startTime.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionZeroRoomId()
    {
      const int zeroRoomId = 0;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, 2, zeroRoomId, _startTime, _startTime.AddHours(3), 3, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativeRoomId()
    {
      const int negativeRoomId = -1;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, 2, negativeRoomId, _startTime, _startTime.AddHours(3), 3, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionZeroAppointmentTypeId()
    {
      const int zeroAppointmentTypeId = 0;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, 2, 3, _startTime, _startTime.AddHours(3), zeroAppointmentTypeId, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativeAppointmentTypeId()
    {
      const int negativeAppointmentTypeId = -1;
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, 2, 3, _startTime, _startTime.AddHours(3), negativeAppointmentTypeId, 4, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNullTitle()
    {
      const string nullTitle = null;
      Assert.Throws<ArgumentNullException>(() =>
        Appointment.Create(_scheduleId, 1, 2, 3, _startTime, _startTime.AddHours(3), 4, 5, nullTitle));
    }

    [Fact]
    public void ThrowExceptionEmptyTitle()
    {
      const string emptyTitle = "";
      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(_scheduleId, 1, 1, 1, _startTime, _startTime.AddHours(3), 1, 1, emptyTitle));
    }
  }
}
