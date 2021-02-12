namespace BlazorShared.Models.Room
{
  public class UpdateRoomRequest : BaseRequest
  {
    public int RoomId { get; set; }
    public string Name { get; set; }
  }
}