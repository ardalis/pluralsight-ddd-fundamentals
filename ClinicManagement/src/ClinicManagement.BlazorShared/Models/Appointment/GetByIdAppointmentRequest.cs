using System;

namespace BlazorShared.Models.Appointment
{
  public class GetByIdAppointmentRequest : BaseRequest
  {
    public Guid AppointmentId { get; set; }
  }
}
