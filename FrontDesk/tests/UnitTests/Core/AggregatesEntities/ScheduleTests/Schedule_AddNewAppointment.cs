using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_AddNewAppointment
  {
    private Fixture _fixture = new Fixture();

    [Fact]
    public async Task ThrowsGivenDuplicateAppointment()
    {

    }
    [Fact]
    public async Task MarksConflictingAppointments()
    {

    }
    [Fact]
    public async Task AddsAppointmentScheduledEvent()
    {

    }

  }
}
