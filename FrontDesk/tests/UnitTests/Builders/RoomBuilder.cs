using FrontDesk.Core.SyncedAggregates;

namespace UnitTests.Builders
{
  public class RoomBuilder
  {
    public const string DEFAULT_NAME = "Test Room";
    private Room _room;
    private string _name = DEFAULT_NAME;
    private int _id;

    public RoomBuilder()
    {
    }

    public RoomBuilder WithId(int id)
    {
      _id = id;
      return this;
    }

    public RoomBuilder WithName(string name)
    {
      _name = name;
      return this;
    }

    public RoomBuilder WithDefaultValues()
    {
      _room = new Room(0, DEFAULT_NAME);

      return this;
    }

    public Room Build() => new Room(_id, _name);
  }
}
