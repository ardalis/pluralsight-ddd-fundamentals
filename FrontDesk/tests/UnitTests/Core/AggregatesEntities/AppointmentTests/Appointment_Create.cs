using System;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Create
  {
    private Fixture _fixture = new Fixture();

    [Fact]
    public void CreateSuccess()
    {
      var scheduleId = Guid.NewGuid();
      var appointment = Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test");

      Assert.Null(appointment.DateTimeConfirmed);
      Assert.Equal(1, appointment.AppointmentTypeId);
      Assert.Equal(scheduleId, appointment.ScheduleId);
      Assert.Equal(1, appointment.ClientId);
      Assert.Equal(1, appointment.DoctorId);
      Assert.Equal(1, appointment.PatientId);
      Assert.Equal(1, appointment.RoomId);
      Assert.Equal(3 * 60, appointment.TimeRange.DurationInMinutes());
      Assert.Equal("Title Test", appointment.Title);
    }

    [Fact]
    public void ThrowExceptionZeroClientId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 0, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativeClientId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, -1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionZeroPatientId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, 0, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativePatientId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, -1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionZeroRoomId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, 1, 0, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativeRoomId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, 1, -1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionZeroAppointmentTypeId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 0, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNegativeAppointmentTypeId()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), -1, 1, "Title Test"));
    }

    [Fact]
    public void ThrowExceptionNullTitle()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentNullException>(() =>
        Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, null));
    }

    [Fact]
    public void ThrowExceptionEmptyTitle()
    {
      var scheduleId = Guid.NewGuid();

      Assert.Throws<ArgumentException>(() =>
        Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, ""));
    }
  }
}
