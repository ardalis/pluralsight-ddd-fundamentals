using System;
using System.Linq;
using AutoFixture;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_Confirm
  {
    private Fixture _fixture = new Fixture();

    [Fact]
    public void ReconfirmResultDateNotChanged()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();

      var confirmedDate = DateTime.Now;

      appointment.Confirm(confirmedDate);

      Assert.NotEqual(confirmedDate, appointment.DateTimeConfirmed);
    }

    [Fact]
    public void ConfirmFirstTimeResultConfirmDateUpdated()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();
      appointment.DateTimeConfirmed = null;

      var confirmedDate = DateTime.Now;

      appointment.Confirm(confirmedDate);

      Assert.Equal(confirmedDate, appointment.DateTimeConfirmed);
    }

    [Fact]
    public void ConfirmFirstTimeResultEventCreated()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(z => z.Events)
        .Create();
      appointment.DateTimeConfirmed = null;

      var confirmedDate = DateTime.Now;

      appointment.Confirm(confirmedDate);

      Assert.NotNull(appointment.Events.FirstOrDefault(x => x.GetType() == typeof(AppointmentConfirmedEvent)));
    }
  }
}
