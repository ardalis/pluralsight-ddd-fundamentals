using AutoFixture;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.AppointmentTests
{
  public class Appointment_UpdateRoom
  {
    private Fixture _fixture = new Fixture();

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
