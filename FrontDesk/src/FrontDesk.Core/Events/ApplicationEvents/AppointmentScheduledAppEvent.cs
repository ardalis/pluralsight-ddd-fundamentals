using System;
using BlazorShared.Models.Appointment;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events.ApplicationEvents
{
  public class AppointmentScheduledAppEvent : BaseDomainEvent
  {
    public AppointmentScheduledAppEvent()
    {
      DateOccurred = DateTime.Now;
    }

    public AppointmentScheduledDTO AppointmentScheduled { get; set; }
    public string EventType
    {
      get
      {
        return nameof(AppointmentScheduledAppEvent);
      }
    }

    public class AppointmentScheduledDTO
    {
      public Guid AppointmentId { get; set; }
      public string ClientName { get; set; }
      public string ClientEmailAddress { get; set; }
      public string PatientName { get; set; }
      public string DoctorName { get; set; }
      public string AppointmentType { get; set; }
      public DateTime Start { get; set; }
      public DateTime End { get; set; }
    }
  }
}
