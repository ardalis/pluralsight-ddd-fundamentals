using System;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events.IntegrationEvents
{
  public class AppointmentScheduledIntegrationEvent : BaseIntegrationEvent
  {
    public AppointmentScheduledIntegrationEvent()
    {
      DateOccurred = DateTimeOffset.Now;
    }

    public Guid AppointmentId { get; set; }
    public string ClientName { get; set; }
    public string ClientEmailAddress { get; set; }
    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public string AppointmentType { get; set; }
    public DateTimeOffset AppointmentStartDateTime { get; set; }
    public string EventType => nameof(AppointmentScheduledIntegrationEvent);
  }
}
