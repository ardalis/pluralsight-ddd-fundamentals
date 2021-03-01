using System;

namespace BlazorShared.Models.Appointment
{
  public class ListAppointmentRequest : BaseRequest
  {
    public Guid ScheduleId { get; set; }
  }
}
