using System;
using AutoFixture;
using AutoFixture.Kernel;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Confirm
  {
    private readonly Fixture _fixture = new Fixture();
    private readonly DateTime _confirmedDate = new DateTime(2021, 01, 01);

    public Appointment_Confirm()
    {
      _fixture.Customizations.Add(
        new FilteringSpecimenBuilder(
          new FixedBuilder(new DateTimeRange(DateTime.Today.AddHours(12),
            DateTime.Today.AddHours(13))),
          new ParameterSpecification(
            typeof(DateTimeRange), "timeRange")));
    }

    [Fact]
    public void ReconfirmResultDateNotChanged()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();

      appointment.Confirm(_confirmedDate);

      Assert.NotEqual(_confirmedDate, appointment.DateTimeConfirmed);
    }

    [Fact]
    public void ConfirmFirstTimeResultConfirmDateUpdated()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();
      appointment.DateTimeConfirmed = null;

      appointment.Confirm(_confirmedDate);

      Assert.Equal(_confirmedDate, appointment.DateTimeConfirmed);
    }

    [Fact]
    public void ConfirmFirstTimeResultEventCreated()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();
      appointment.DateTimeConfirmed = null;

      appointment.Confirm(_confirmedDate);

      Assert.Contains(appointment.Events, x => x.GetType() == typeof(AppointmentConfirmedEvent));
    }
  }
}
