using System;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateEndTime
  {
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime;
    private readonly Fixture _fixture = new Fixture();
    private readonly string _testAppointmentTypeTitle = "Test Type";
    private readonly int _duration = 30;

    public Appointment_UpdateEndTime()
    {
      _endTime = _startTime.AddHours(3);
    }

    [Fact]
    public void SuccessWhenUpdateEndTime()
    {
      var scheduleId = Guid.NewGuid();
      var testClientId = 1;
      var testPatientId = 1;
      var testRoomId = 3;
      var testAppointmentTypeId = 4;
      var testDoctorId = 5;
      var testTitle = "Test Title";

      var appointment =
        Appointment.Create(scheduleId, testClientId, testPatientId, testRoomId, _startTime, _endTime, testAppointmentTypeId, testDoctorId, testTitle);
      appointment.UpdateEndTime(new AppointmentType(1, _testAppointmentTypeTitle, "01", _duration));

      Assert.Equal(30, appointment.TimeRange.DurationInMinutes());
    }

    [Fact]
    public void ThrowGivenNullStartTime()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(a => a.Events)
        .Create();

      Assert.Throws<ArgumentNullException>(() =>
        appointment.UpdateEndTime(new AppointmentType(1, _testAppointmentTypeTitle, "01", _duration)));
    }
  }
}
