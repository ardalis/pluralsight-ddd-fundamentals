using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateStartTime
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
      var dateTimeRange = new DateTimeOffsetRange(_startTime, _endTime);

      var appointment = new Appointment(appointmentTypeId, scheduleId, clientId, doctorId, patientId, roomId, dateTimeRange, title, null);

      var newStartTime = new DateTime(2021, 01, 01, 11, 00, 00);

      appointment.UpdateStartTime(newStartTime);

      Assert.Equal(dateTimeRange.DurationInMinutes(), appointment.TimeRange.DurationInMinutes());
      Assert.Equal(newStartTime, appointment.TimeRange.Start);
    }
  }
}
