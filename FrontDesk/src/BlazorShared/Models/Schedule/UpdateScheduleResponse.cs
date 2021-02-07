using System;

namespace BlazorShared.Models.Schedule
{
  public class UpdateScheduleResponse : BaseResponse
  {
    public ScheduleDto Schedule { get; set; } = new ScheduleDto();

    public UpdateScheduleResponse(Guid correlationId) : base(correlationId)
    {
    }

    public UpdateScheduleResponse()
    {
    }
  }
}
