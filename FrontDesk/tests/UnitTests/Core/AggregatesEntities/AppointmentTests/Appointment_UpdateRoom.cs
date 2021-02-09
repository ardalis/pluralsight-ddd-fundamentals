using System.Linq;
using System.Threading.Tasks;
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
    public async Task DoesNothingGivenSameRoomId()
    {
      var appt = _fixture.Build<Appointment>()
        .Without(a => a.Events)
        .Create();
      var initialRoomId = appt.RoomId;
      var initialEventCount = appt.Events.Count;

      appt.UpdateRoom(initialRoomId);

      Assert.Equal(initialRoomId, appt.RoomId);
      Assert.Equal(initialEventCount, appt.Events.Count);
    }

    [Fact]
    public async Task UpdatesRoomIdAndAddsEventGivenNewRoomId()
    {
      var appt = _fixture.Build<Appointment>()
        .Without(a => a.Events)
        .Create();
      var initialRoomId = appt.RoomId;
      var initialEventCount = appt.Events.Count;

      int newRoomId = initialRoomId + 1;
      appt.UpdateRoom(newRoomId);

      Assert.Equal(newRoomId, appt.RoomId);
      Assert.Single(appt.Events);
      Assert.Contains(appt.Events, x => x.GetType() == typeof(AppointmentUpdatedEvent));
    }
  }
}
