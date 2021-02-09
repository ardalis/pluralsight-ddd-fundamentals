using System;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateEndTime
  {
    private Fixture _fixture = new Fixture();

    [Fact]
    public void SuccessWhenUpdateEndTime()
    {
      var scheduleId = Guid.NewGuid();

      var appointment =
        Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test");
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
