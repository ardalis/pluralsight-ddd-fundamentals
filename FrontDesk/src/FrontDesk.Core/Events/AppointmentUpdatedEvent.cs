using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentUpdatedEvent : BaseDomainEvent
  {
    public AppointmentUpdatedEvent(Appointment appointment)
        : this()
    {
      AppointmentUpdated = appointment;
    }
    public AppointmentUpdatedEvent()
    {
      this.Id = Guid.NewGuid();
      DateTimeEventOccurred = DateTime.Now;
    }

    public Guid Id { get; private set; }
    public DateTime DateTimeEventOccurred { get; private set; }
    public Appointment AppointmentUpdated { get; private set; }
  }
}
