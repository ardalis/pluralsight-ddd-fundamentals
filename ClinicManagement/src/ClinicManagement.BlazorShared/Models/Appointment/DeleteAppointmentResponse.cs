using System;

namespace BlazorShared.Models.Appointment
{
  public class DeleteAppointmentResponse : BaseResponse
  {

    public DeleteAppointmentResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteAppointmentResponse()
    {
    }
  }
}
