using System;
using AutoFixture;
using AutoFixture.Kernel;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using Xunit;
using FrontDesk.Core.ScheduleAggregate;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateRoom
  {
    private readonly Fixture _fixture = new Fixture();
    public Appointment_UpdateRoom()
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
    public void DoesNothingGivenSameRoomId()
    {
      var appointment = GetUnconfirmedAppointment();
      var initialRoomId = appointment.RoomId;
      var initialEventCount = appointment.Events.Count;

      appointment.UpdateRoom(initialRoomId);

      Assert.Equal(initialRoomId, appointment.RoomId);
      Assert.Equal(initialEventCount, appointment.Events.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowsGivenNegativeOrZeroRoomId(int invalidRoomId)
    {
      var appointment = GetUnconfirmedAppointment();

      Action action = () => appointment.UpdateRoom(invalidRoomId);

      Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void UpdatesRoomIdAndAddsEventGivenNewRoomId()
    {
      var appointment = GetUnconfirmedAppointment();
      var initialRoomId = appointment.RoomId;
      var initialEventCount = appointment.Events.Count;

      int newRoomId = initialRoomId + 1;
      appointment.UpdateRoom(newRoomId);

      Assert.Equal(newRoomId, appointment.RoomId);
      Assert.Single(appointment.Events);
      Assert.Contains(appointment.Events, x => x.GetType() == typeof(AppointmentUpdatedEvent));
    }
  }
}
