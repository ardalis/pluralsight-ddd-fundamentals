using System;

namespace BlazorShared.Models.Schedule
{
  public class GetByIdScheduleRequest : BaseRequest
  {
    public const string Route = "api/schedules/{scheduleId}";
    public Guid ScheduleId { get; set; }
  }
}
