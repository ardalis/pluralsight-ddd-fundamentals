using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentScheduledEvent : BaseDomainEvent
  {
    public AppointmentScheduledEvent(Appointment appointment) : this()
    {
      AppointmentScheduled = appointment;
    }

    public AppointmentScheduledEvent()
    {
      this.Id = Guid.NewGuid();
      DateTimeEventOccurred = DateTime.Now;
    }

    public Guid Id { get; private set; }

    public DateTime DateTimeEventOccurred { get; private set; }

    public Appointment AppointmentScheduled { get; private set; }
  }
}
