using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentScheduledEvent : BaseDomainEvent
  {
    public AppointmentScheduledEvent(Appointment appointment)
    {
      AppointmentScheduled = appointment;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Appointment AppointmentScheduled { get; private set; }
  }
}
