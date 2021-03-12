using System;
using AutoFixture;
using AutoFixture.Kernel;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateRoom
  {
    private readonly Fixture _fixture = new Fixture();
    public Appointment_UpdateRoom()
    {
      _fixture.Customizations.Add(
        new FilteringSpecimenBuilder(
          new FixedBuilder(new DateTimeRange(DateTime.Today.AddHours(12),
            DateTime.Today.AddHours(13))),
          new ParameterSpecification(
            typeof(DateTimeRange), "timeRange")));
    }

    [Fact]
    public void DoesNothingGivenSameRoomId()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(a => a.Events)
        .Create();
      var initialRoomId = appointment.RoomId;
      var initialEventCount = appointment.Events.Count;

      appointment.UpdateRoom(initialRoomId);

      Assert.Equal(initialRoomId, appointment.RoomId);
      Assert.Equal(initialEventCount, appointment.Events.Count);
    }

    [Fact]
    public void UpdatesRoomIdAndAddsEventGivenNewRoomId()
    {
      var appointment = _fixture.Build<Appointment>()
        .Without(a => a.Events)
        .Create();
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
