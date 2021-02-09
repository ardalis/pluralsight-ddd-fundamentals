using System;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Confirm
  {
    private readonly Fixture _fixture = new Fixture();
    private DateTime _confirmedDate = new DateTime(2021, 01, 01);

    [Fact]
    public void ReconfirmResultDateNotChanged()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();

      _confirmedDate = new DateTime(2021, 01, 01);

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

      _confirmedDate = new DateTime(2021, 01, 01);

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

      _confirmedDate = new DateTime(2021, 01, 01);

      appointment.Confirm(_confirmedDate);

      Assert.Contains(appointment.Events, x => x.GetType() == typeof(AppointmentConfirmedEvent));
    }
  }
}
