using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentConfirmedEvent : BaseDomainEvent
  {
    public AppointmentConfirmedEvent(Appointment appointment) : this()
    {
      AppointmentUpdated = appointment;
    }

    public AppointmentConfirmedEvent()
    {
      this.Id = Guid.NewGuid();
      DateTimeEventOccurred = DateTime.Now;
    }

    public Guid Id { get; private set; }

    public DateTime DateTimeEventOccurred { get; private set; }

    public Appointment AppointmentUpdated { get; private set; }
  }
}
