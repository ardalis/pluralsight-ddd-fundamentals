using System;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateEndTime
  {
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void SuccessWhenUpdateEndTime()
    {
      var scheduleId = Guid.NewGuid();

      var appointment =
        Appointment.Create(scheduleId, 1, 1, 1, _startTime, _startTime.AddHours(3), 1, 1, "Title Test");
      appointment.UpdateEndTime(new AppointmentType(1, "Test Type", "01", 30));

      Assert.Equal(30, appointment.TimeRange.DurationInMinutes());
    }

    [Fact]
    public void ThrowGivenNullStartTime()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(a => a.Events)
        .Create();

      Assert.Throws<ArgumentNullException>(() =>
        appointment.UpdateEndTime(new AppointmentType(1, "Test Type", "01", 30)));
    }
  }
}
