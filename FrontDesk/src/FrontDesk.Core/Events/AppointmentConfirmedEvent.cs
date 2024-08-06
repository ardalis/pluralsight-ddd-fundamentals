using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Events
{
  public class AppointmentConfirmedEvent : AppointmentEvent
  {
    public AppointmentConfirmedEvent(Appointment appointment) : base(appointment)
    {
    }
  }
}
