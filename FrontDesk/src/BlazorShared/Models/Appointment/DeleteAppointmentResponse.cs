using System;
using BlazorShared.Models.Schedule;

namespace BlazorShared.Models.Appointment
{
  public class DeleteAppointmentResponse : BaseResponse
  {

    public ScheduleDto Schedule { get; set; }

    public DeleteAppointmentResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteAppointmentResponse()
    {
    }

  }
}
