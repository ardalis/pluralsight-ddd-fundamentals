using System;

namespace BlazorShared.Models.Room
{
  public class DeleteRoomResponse : BaseResponse
  {
    public DeleteRoomResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteRoomResponse()
    {
    }
  }
}