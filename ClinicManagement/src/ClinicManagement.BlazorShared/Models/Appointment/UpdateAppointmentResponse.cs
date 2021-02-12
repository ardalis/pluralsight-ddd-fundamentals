using System;

namespace BlazorShared.Models.Appointment
{
  public class UpdateAppointmentResponse : BaseResponse
  {
    public AppointmentDto Appointment { get; set; } = new AppointmentDto();

    public UpdateAppointmentResponse(Guid correlationId) : base(correlationId)
    {
    }

    public UpdateAppointmentResponse()
    {
    }
  }
}
