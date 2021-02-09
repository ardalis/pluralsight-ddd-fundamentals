using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateTime
  {
    [Fact]
    public void UpdatesTimeSucceeded()
    {
      var scheduleId = Guid.NewGuid();

      var appointment =
        Appointment.Create(scheduleId, 1, 2, 3, DateTime.Now, DateTime.Now.AddHours(3), 4, 5, "Title Test");

      var startTime = new DateTime(2021, 01, 01, 10, 00, 00);
      var range = new DateTimeRange(startTime, startTime.AddHours(1));

      appointment.UpdateTime(range);

      Assert.Equal(range.DurationInMinutes(), appointment.TimeRange.DurationInMinutes());
    }
  }
}
