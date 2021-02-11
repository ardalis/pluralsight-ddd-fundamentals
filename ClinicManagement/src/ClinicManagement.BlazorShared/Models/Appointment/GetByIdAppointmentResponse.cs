using System;

namespace BlazorShared.Models.Appointment
{
  public class GetByIdAppointmentResponse : BaseResponse
  {
    public AppointmentDto Appointment { get; set; } = new AppointmentDto();

    public GetByIdAppointmentResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetByIdAppointmentResponse()
    {
    }
  }
}
