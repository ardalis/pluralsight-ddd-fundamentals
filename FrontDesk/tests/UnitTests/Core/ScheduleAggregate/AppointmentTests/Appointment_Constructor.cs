using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Constructor
  {
    [Fact]
    public void ConstructorCreatedAndIdSetToEmptyGuid()
    {
      const int appointmentTypeId = 1;
      var scheduleId = Guid.NewGuid();
      var appointmentId = Guid.NewGuid();
      const int clientId = 2;
      const int doctorId = 3;
      const int patientId = 4;
      const int roomId = 5;
      const string title = "Title Test";
      var startTime = new DateTimeOffset(2021, 01, 01, 10, 00, 00, new TimeSpan(-4, 0, 0));
      var endTime = startTime.AddHours(3);
      var range = new DateTimeOffsetRange(startTime, endTime);
      const int threeHours = 3 * 60;

      var appointment = new Appointment(appointmentId, appointmentTypeId, scheduleId, clientId, doctorId, patientId, roomId, range, title);

      Assert.Null(appointment.DateTimeConfirmed);
      Assert.Equal(appointmentTypeId, appointment.AppointmentTypeId);
      Assert.Equal(scheduleId, appointment.ScheduleId);
      Assert.Equal(clientId, appointment.ClientId);
      Assert.Equal(doctorId, appointment.DoctorId);
      Assert.Equal(patientId, appointment.PatientId);
      Assert.Equal(roomId, appointment.RoomId);
      Assert.Equal(threeHours, appointment.TimeRange.DurationInMinutes());
      Assert.Equal(title, appointment.Title);
      Assert.Equal(appointmentId, appointment.Id);
    }
  }
}
