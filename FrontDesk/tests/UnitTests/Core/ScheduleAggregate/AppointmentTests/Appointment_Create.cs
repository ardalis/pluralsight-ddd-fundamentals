using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Create
  {
    private readonly DateTimeOffset _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTimeOffset _endTime;
    private readonly DateTimeOffsetRange _range;
    private readonly Guid _scheduleId = Guid.NewGuid();
    private readonly Guid _appointmentId = Guid.NewGuid();
    private readonly int _testClientId = 1;
    private readonly int _testPatientId = 2;
    private readonly int _testRoomId = 3;
    private readonly int _testAppointmentTypeId = 4;
    private readonly int _testDoctorId = 5;
    private readonly string _testTitle = "Test Title";

    public Appointment_Create()
    {
      _endTime = _startTime.AddHours(3);
      _range = new DateTimeOffsetRange(_startTime, _endTime);
    }

    [Fact]
    public void CreateSuccess()
    {
      var appointment = new Appointment(_appointmentId, _testAppointmentTypeId, _scheduleId, _testClientId, _testDoctorId, _testPatientId, _testRoomId, _range, _testTitle);
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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowsExceptionGivenInvalidClientId(int invalidClientId)
    {
      void Action() => new Appointment(_appointmentId, _testAppointmentTypeId, _scheduleId, invalidClientId, _testDoctorId, _testPatientId, _testRoomId, _range, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowsExceptionGivenInvalidPatientId(int invalidPatientId)
    {
      void Action() => new Appointment(_appointmentId, _testAppointmentTypeId, _scheduleId, _testClientId, _testDoctorId, invalidPatientId, _testRoomId, _range, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowsExceptionGivenInvalidRoomId(int invalidRoomId)
    {
      void Action() => new Appointment(_appointmentId, _testAppointmentTypeId, _scheduleId, _testClientId, _testDoctorId, _testPatientId, invalidRoomId, _range, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowsExceptionInvalidAppointmentTypeId(int invalidAppointmentTypeId)
    {
      void Action() => new Appointment(_appointmentId, invalidAppointmentTypeId, _scheduleId, _testClientId, _testDoctorId, _testPatientId, _testRoomId, _range, _testTitle);

      Assert.Throws<ArgumentException>(Action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ThrowsExceptionGivenInvalidTitle(string invalidTitle)
    {
      void Action() => new Appointment(_appointmentId, _testAppointmentTypeId, _scheduleId, _testClientId, _testDoctorId, _testPatientId, _testRoomId, _range, invalidTitle);

      if (invalidTitle == null)
      {
        Assert.Throws<ArgumentNullException>(Action);
      } else
      {
        Assert.Throws<ArgumentException>(Action);
      }
    }
  }
}
