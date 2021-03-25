using System;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events.ApplicationEvents
{
  public class AppointmentConfirmedAppEvent : BaseDomainEvent
  {
    public AppointmentConfirmedAppEvent()
    {
      this.Id = Guid.NewGuid();
      DateOccurred = DateTime.Now;
    }

    public Guid Id { get; private set; }
    public Guid AppointmentId { get; set; }
    public string EventType
    {
      get
      {
        return nameof(AppointmentConfirmedAppEvent);
      }
    }
  }
}
