using System;
using AutoFixture;
using AutoFixture.Kernel;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using Xunit;
using FrontDesk.Core.ScheduleAggregate;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateDoctor
  {
    private readonly Fixture _fixture = new Fixture();
    public Appointment_UpdateDoctor()
    {
      _fixture.Customizations.Add(
        new FilteringSpecimenBuilder(
          new FixedBuilder(new DateTimeOffsetRange(DateTime.Today.AddHours(12),
            DateTime.Today.AddHours(13))),
          new ParameterSpecification(
            typeof(DateTimeOffsetRange), "timeRange")));
    }

    private Appointment GetUnconfirmedAppointment()
    {
      var newAppt = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();
      newAppt.DateTimeConfirmed = null;
      return newAppt;
    }

    [Fact]
    public void DoesNothingGivenSameDoctorId()
    {
      var appointment = GetUnconfirmedAppointment();
      var initialDoctorId = appointment.DoctorId;
      var initialEventCount = appointment.Events.Count;

      appointment.UpdateDoctor(initialDoctorId);

      Assert.Equal(initialDoctorId, appointment.DoctorId);
      Assert.Equal(initialEventCount, appointment.Events.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowsGivenNegativeOrZeroDoctorId(int invalidDoctorId)
    {
      var appointment = GetUnconfirmedAppointment();

      Action action = () => appointment.UpdateDoctor(invalidDoctorId);

      Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void UpdatesDoctorIdAndAddsEventGivenNewDoctorId()
    {
      var appointment = GetUnconfirmedAppointment();
      var initialDoctorId = appointment.DoctorId;
      var initialEventCount = appointment.Events.Count;

      int newDoctorId = initialDoctorId + 1;
      appointment.UpdateDoctor(newDoctorId);

      Assert.Equal(newDoctorId, appointment.DoctorId);
      Assert.Single(appointment.Events);
      Assert.Contains(appointment.Events, x => x.GetType() == typeof(AppointmentUpdatedEvent));
    }
  }
}
