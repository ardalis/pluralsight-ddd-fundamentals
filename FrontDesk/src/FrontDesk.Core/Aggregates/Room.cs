using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Room : BaseEntity<int>, IAggregateRoot
  {
    public virtual string Name { get; private set; }

    public Room(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public override string ToString()
    {
      return Name.ToString();
    }

    public Room UpdateName(string name)
    {
      Name = name;
      return this;
    }

    private Room() // required for EF
    {
    }
  }
}
