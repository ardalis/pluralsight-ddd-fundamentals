using ClinicManagement.Core.Aggregates;

namespace UnitTests.Builders
{
  public class RoomBuilder
  {
    private Room _room;

    public RoomBuilder()
    {
      WithDefaultValues();
    }

    public RoomBuilder Id(int id)
    {
      _room.Id = id;
      return this;
    }

    public RoomBuilder SetRoom(Room room)
    {
      _room = room;
      return this;
    }

    public RoomBuilder WithDefaultValues()
    {
      _room = new Room(1, "Test Room");

      return this;
    }

    public Room Build() => _room;
  }
}
