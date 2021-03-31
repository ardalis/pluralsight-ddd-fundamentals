using System;
using FrontDesk.Core.Aggregates;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  // This is fired from within the model when after an appt was confirmed
  public class AppointmentConfirmedEvent : BaseDomainEvent
  {
    public AppointmentConfirmedEvent(Appointment appointment) : this()
    {
      AppointmentUpdated = appointment;
    }

    public AppointmentConfirmedEvent()
    {
      this.Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }

    public Appointment AppointmentUpdated { get; private set; }
  }
}
