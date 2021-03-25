using System;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Events.ApplicationEvents
{
  public class CreateConfirmationEmailMessage : IApplicationEvent
  {
    public Guid AppointmentId { get; set; }
    public string ClientName { get; set; }
    public string ClientEmailAddress { get; set; }
    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public string AppointmentType { get; set; }
    public DateTime AppointmentStartDateTime { get; set; }

    public string EventType => nameof(CreateConfirmationEmailMessage);
  }
}
