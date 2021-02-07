using System;
using System.Collections.Generic;

namespace BlazorShared.Models.Schedule
{
  public class ListScheduleResponse : BaseResponse
  {
    public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();

    public int Count { get; set; }

    public ListScheduleResponse(Guid correlationId) : base(correlationId)
    {
    }

    public ListScheduleResponse()
    {
    }
  }
}
