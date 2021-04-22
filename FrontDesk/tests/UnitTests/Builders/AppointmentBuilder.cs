using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace UnitTests.Builders
{
  public class AppointmentBuilder
  {
    public const int TEST_APPOINTMENT_TYPE_ID = 1;
    public readonly Guid TEST_SCHEDULE_ID = Guid.Parse("f9369039-9d11-4442-9738-ed65d8a8ad52");
    public const int TEST_CLIENT_ID = 2;
    public const int TEST_PATIENT_ID = 3;
    public const int TEST_DOCTOR_ID = 4;
    public const int TEST_ROOM_ID = 5;
    public const string TEST_TITLE = "Test Title";
    public static readonly DateTime TEST_START_TIME = new DateTime(2021, 01, 01, 10, 00, 00);

    private Guid _scheduleId;
    private Guid _id = Guid.NewGuid();
    private int _appointmentTypeId;
    private int _clientId;
    private int _doctorId;
    private int _patientId;
    private int _roomId;
    private DateTimeOffsetRange _dateTimeOffsetRange;
    private string _title;

    public AppointmentBuilder()
    {
    }

    public AppointmentBuilder WithId(Guid id)
    {
      _id = id;
      return this;
    }

    public AppointmentBuilder WithDateTimeOffsetRange(DateTimeOffsetRange dateTimeOffsetRange)
    {
      _dateTimeOffsetRange = dateTimeOffsetRange;
      return this;
    }

    public AppointmentBuilder WithDefaultValues()
    {
      _id = Guid.NewGuid();
      _appointmentTypeId = TEST_APPOINTMENT_TYPE_ID;
      _scheduleId = TEST_SCHEDULE_ID;
      _clientId = TEST_CLIENT_ID;
      _doctorId = TEST_DOCTOR_ID;
      _patientId = TEST_PATIENT_ID;
      _roomId = TEST_ROOM_ID;

      var startTime = TEST_START_TIME;
      var endTime = new DateTime(2021, 01, 01, 12, 00, 00);
      _dateTimeOffsetRange = new DateTimeOffsetRange(startTime, endTime);

      _title = TEST_TITLE;

      return this;
    }

    public Appointment Build()
    {
      return new Appointment(_id, _appointmentTypeId, _scheduleId, _clientId, _doctorId, _patientId, _roomId, _dateTimeOffsetRange, _title);
    }
  }
}
