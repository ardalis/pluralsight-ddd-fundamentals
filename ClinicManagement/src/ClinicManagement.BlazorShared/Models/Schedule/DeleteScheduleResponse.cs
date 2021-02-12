using System;

namespace BlazorShared.Models.Schedule
{
  public class DeleteScheduleResponse : BaseResponse
  {

    public DeleteScheduleResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteScheduleResponse()
    {
    }
  }
}
