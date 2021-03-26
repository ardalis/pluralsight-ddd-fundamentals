using System;

namespace BlazorShared.Models.Appointment
{
  public class ListAppointmentRequest : BaseRequest
  {
    public const string Route = "api/schedule/{ScheduleId}/appointments";
    public Guid ScheduleId { get; set; }
  }
}
