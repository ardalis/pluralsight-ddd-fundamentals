using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Events
{
  public class AppointmentUpdatedEvent : AppointmentEvent
  {
    public AppointmentUpdatedEvent(Appointment appointment) : base(appointment)
    {
    }
  }
}
