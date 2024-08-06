using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Events
{
  public class AppointmentScheduledEvent : AppointmentEvent
  {
    public AppointmentScheduledEvent(Appointment appointment) : base(appointment)
    {
    }
  }
}
