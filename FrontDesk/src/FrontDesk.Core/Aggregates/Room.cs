using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Room : BaseEntity<int>, IAggregateRoot
  {
    public virtual string Name { get; private set; }

    public Room(string name)
    {
      Name = name;
    }

    public Room(int id)
    {
      Id = id;
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
