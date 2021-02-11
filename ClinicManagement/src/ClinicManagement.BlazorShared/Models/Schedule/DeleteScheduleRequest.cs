using System;

namespace BlazorShared.Models.Schedule
{
  public class DeleteScheduleRequest : BaseRequest
  {
    public Guid Id { get; set; }
  }
}
