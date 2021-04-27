using System;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events.IntegrationEvents
{
  // This is fired by the message queue handler when an appointment should
  // be marked confirmed. It happens before the appointment is confirmed in
  // the model.
  public class AppointmentConfirmedAppEvent : BaseDomainEvent
  {
    public AppointmentConfirmedAppEvent() : this(DateTime.Now)
    {
    }

    public AppointmentConfirmedAppEvent(DateTimeOffset dateOccurred)
    {
      DateOccurred = dateOccurred.DateTime;
    }

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
