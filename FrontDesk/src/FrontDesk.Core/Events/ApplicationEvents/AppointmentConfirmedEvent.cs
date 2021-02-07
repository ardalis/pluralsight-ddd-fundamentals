using System;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events.ApplicationEvents
{
  public class AppointmentConfirmedEvent : BaseDomainEvent
  {
    public AppointmentConfirmedEvent()
    {
      this.Id = Guid.NewGuid();
      DateTimeEventOccurred = DateTime.Now;
    }

    public Guid Id { get; private set; }
    public DateTime DateTimeEventOccurred { get; set; }
    public Guid AppointmentId { get; set; }
    public string EventType
    {
      get
      {
        return nameof(AppointmentConfirmedEvent);
      }
    }
  }
}
