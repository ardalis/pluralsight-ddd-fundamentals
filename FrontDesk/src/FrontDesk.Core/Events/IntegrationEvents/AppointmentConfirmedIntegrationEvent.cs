using System;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events.IntegrationEvents
{
  // This is fired by the message queue handler when an appointment should
  // be marked confirmed. It happens before the appointment is confirmed in
  // the model.
  public class AppointmentConfirmedIntegrationEvent : BaseIntegrationEvent
  {
    public AppointmentConfirmedIntegrationEvent() : this(DateTimeOffset.Now)
    {
    }

    public AppointmentConfirmedIntegrationEvent(DateTimeOffset dateOccurred)
    {
      DateOccurred = dateOccurred;
    }

    public Guid AppointmentId { get; set; }
    public string EventType
    {
      get
      {
        return nameof(AppointmentConfirmedIntegrationEvent);
      }
    }
  }
}
