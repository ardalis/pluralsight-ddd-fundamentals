using System;

namespace BlazorShared.Models.Appointment
{
  public class DeleteAppointmentRequest : BaseRequest
  {
    public Guid ScheduleId { get; set; }
    public Guid AppointmentId { get; set; }
  }
}
