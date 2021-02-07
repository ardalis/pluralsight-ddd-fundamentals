using System;

namespace BlazorShared.Models.Schedule
{
  public class GetByIdScheduleResponse : BaseResponse
  {
    public ScheduleDto Schedule { get; set; } = new ScheduleDto();

    public GetByIdScheduleResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetByIdScheduleResponse()
    {
    }
  }
}
