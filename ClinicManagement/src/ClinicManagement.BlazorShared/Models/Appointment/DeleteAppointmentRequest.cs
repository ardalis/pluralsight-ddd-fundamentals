using System;

namespace BlazorShared.Models.Appointment
{
  public class DeleteAppointmentRequest : BaseRequest
  {
    public Guid Id { get; set; }
  }
}
