using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentDeletedEvent : BaseDomainEvent
  {
    public AppointmentDeletedEvent(Appointment appointment)
    {
      AppointmentDeleted = appointment;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Appointment AppointmentDeleted { get; private set; }
  }
}
