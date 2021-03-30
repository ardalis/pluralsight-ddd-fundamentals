using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;

namespace UnitTests.Builders
{
  public class AppointmentBuilder
  {
    private Appointment _appointment;

    public AppointmentBuilder()
    {
      WithDefaultValues();
    }

    public AppointmentBuilder Id(Guid id)
    {
      _appointment.Id = id;
      return this;
    }

    public AppointmentBuilder SetAppointment(Appointment appointment)
    {
      _appointment = appointment;
      return this;
    }

    public AppointmentBuilder WithDefaultValues()
    {
      var appointmentTypeId = 1;
      var scheduleId = Guid.Parse("f9369039-9d11-4442-9738-ed65d8a8ad52");
      var clientId = 1;
      var doctorId = 1;
      var patientId = 1;
      var roomId = 1;

      var startTime = new DateTime(2021, 01, 01, 10, 00, 00);
      var endTime = new DateTime(2021, 01, 01, 12, 00, 00);
      var timeRange = new DateTimeOffsetRange(startTime, endTime);

      var title = "Test Title";

      _appointment = new Appointment(appointmentTypeId, scheduleId, clientId, doctorId, patientId, roomId, timeRange, title);

      return this;
    }

    public Appointment Build() => _appointment;
  }
}
