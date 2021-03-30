namespace BlazorShared.Models.Room
{
  public class CreateRoomRequest : BaseRequest
  {
    public string Name { get; set; }

    public override string ToString()
    {
      return $"Room: Name = {Name}";
    }
  }
}
