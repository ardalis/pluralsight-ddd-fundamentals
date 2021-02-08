using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Doctor : BaseEntity<int>, IAggregateRoot
  {
    public virtual string Name { get; private set; }

    public Doctor(int id, string name)
    {
      Id = id;
      Name = name;
    }

    private Doctor() // required for EF
    {
    }

    public void UpdateName(string name)
    {
      Name = name;
    }

    public override string ToString()
    {
      return Name.ToString();
    }
  }
}
