using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public abstract class AppointmentEvent : BaseDomainEvent
  {
    protected AppointmentEvent(Appointment appointment)
    {
      Appointment = appointment;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public Appointment Appointment { get; }
  }
}
