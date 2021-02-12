using System;

namespace BlazorShared.Models.Schedule
{
  public class CreateScheduleResponse : BaseResponse
  {
    public ScheduleDto Schedule { get; set; } = new ScheduleDto();

    public CreateScheduleResponse(Guid correlationId) : base(correlationId)
    {
    }

    public CreateScheduleResponse()
    {
    }
  }
}