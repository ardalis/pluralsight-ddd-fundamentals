using System;
using FrontDesk.Core.ScheduleAggregate;
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
    }

    public Guid Id { get; private set; }
    public Appointment AppointmentUpdated { get; private set; }
  }
}
