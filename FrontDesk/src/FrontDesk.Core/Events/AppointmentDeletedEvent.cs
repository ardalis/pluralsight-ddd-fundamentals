using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Events
{
  public class AppointmentDeletedEvent : AppointmentEvent
  {
    public AppointmentDeletedEvent(Appointment appointment) : base(appointment)
    {
    }
  }
}
