using System;

namespace BlazorShared.Models.Schedule
{
  public class DeleteScheduleRequest : BaseRequest
  {
    public const string Route = "api/schedules/{Id}";
    public Guid Id { get; set; }
  }
}
