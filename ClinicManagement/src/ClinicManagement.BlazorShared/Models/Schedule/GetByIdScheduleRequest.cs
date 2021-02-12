using System;

namespace BlazorShared.Models.Schedule
{
  public class GetByIdScheduleRequest : BaseRequest
  {
    public Guid ScheduleId { get; set; }
  }
}
