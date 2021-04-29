using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentConfirmedEvent : BaseDomainEvent
  {
    public AppointmentConfirmedEvent(Appointment appointment)
    {
      AppointmentUpdated = appointment;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public Appointment AppointmentUpdated { get; private set; }
  }
}
