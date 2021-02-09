using System;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateTime
  {
    private Fixture _fixture = new Fixture();

    [Fact]
    public void SuccessUpdateTime()
    {
      var scheduleId = Guid.NewGuid();

      var appointment =
        Appointment.Create(scheduleId, 1, 1, 1, DateTime.Now, DateTime.Now.AddHours(3), 1, 1, "Title Test");

      var range = new DateTimeRange(DateTime.Now, DateTime.Now.AddHours(1));

      appointment.UpdateTime(range);

      Assert.Equal(range.DurationInMinutes(), appointment.TimeRange.DurationInMinutes());
    }
  }
}
