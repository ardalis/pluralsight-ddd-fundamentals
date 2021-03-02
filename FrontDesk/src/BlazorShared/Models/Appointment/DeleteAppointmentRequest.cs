using System;

namespace BlazorShared.Models.Appointment
{
  public class DeleteAppointmentRequest : BaseRequest
  {
    public const string Route = "api/appointments/{scheduleId}/{appointmentId}";

    public Guid ScheduleId { get; set; }
    public Guid AppointmentId { get; set; }
  }
}
