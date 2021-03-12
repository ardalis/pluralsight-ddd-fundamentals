using System;
using AutoFixture;
using AutoFixture.Kernel;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  //public class Appointment_UpdateEndTime
  //{
  //  private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
  //  private readonly DateTime _endTime;
  //  private readonly Fixture _fixture = new Fixture();
  //  private readonly string _testAppointmentTypeTitle = "Test Type";
  //  private readonly int _duration = 30;
  //  private readonly int _appointmentTypeId = 1;
  //  private readonly string _appointmentTypeCode = "01";

  //  public Appointment_UpdateEndTime()
  //  {
  //    _fixture.Customizations.Add(
  //      new FilteringSpecimenBuilder(
  //        new FixedBuilder(new DateTimeRange(DateTime.Today.AddHours(12),
  //          DateTime.Today.AddHours(13))),
  //        new ParameterSpecification(
  //          typeof(DateTimeRange), "timeRange")));

  //    _endTime = _startTime.AddHours(3);
  //  }

  //  [Fact]
  //  public void SuccessWhenUpdateEndTime()
  //  {
  //    var scheduleId = Guid.NewGuid();
  //    const int testClientId = 1;
  //    const int testPatientId = 2;
  //    const int testRoomId = 3;
  //    const int testAppointmentTypeId = 4;
  //    const int testDoctorId = 5;
  //    const string testTitle = "Test Title";

  //    var appointment =
  //      Appointment.Create(scheduleId, testClientId, testPatientId, testRoomId, _startTime, _endTime, testAppointmentTypeId, testDoctorId, testTitle);
  //    appointment.UpdateEndTime(new AppointmentType(_appointmentTypeId, _testAppointmentTypeTitle, _appointmentTypeCode, _duration));

  //    Assert.Equal(30, appointment.TimeRange.DurationInMinutes());
  //  }

  //  [Fact]
  //  public void ThrowGivenNullStartTime()
  //  {
  //    var appointment = _fixture.Build<Appointment>()
  //      .Without(a => a.Events)
  //      .Create();

  //    Action action = () =>
  //      appointment.UpdateEndTime(new AppointmentType(_appointmentTypeId, _testAppointmentTypeTitle, _appointmentTypeCode, _duration));

  //    Assert.Throws<ArgumentNullException>(action);
  //  }
  //}
}
