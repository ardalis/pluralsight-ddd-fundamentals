using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateTime
  {
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime = new DateTime(2021, 01, 01, 12, 00, 00);

    [Fact]
    public void UpdatesTimeRange()
    {
      var scheduleId = Guid.NewGuid();
      const int clientId = 1;
      const int patientId = 2;
      const int roomId = 3;
      const int appointmentTypeId = 4;
      const int doctorId = 5;
      const string title = "Title Test";

      var appointment =
        Appointment.Create(scheduleId, clientId, patientId, roomId, _startTime, _endTime, appointmentTypeId, doctorId, title);

      var newEndTime = new DateTime(2021, 01, 01, 11, 00, 00);
      var range = new DateTimeRange(_startTime, newEndTime);

      appointment.UpdateTime(range);

      Assert.Equal(range.DurationInMinutes(), appointment.TimeRange.DurationInMinutes());
    }
  }
}
