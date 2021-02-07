using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Doctor : BaseEntity<int>, IAggregateRoot
  {
    public virtual string Name { get; private set; }

    public Doctor(string name)
    {
      Name = name;
    }

    public Doctor(int id)
    {
      Id = id;
    }

    public void UpdateName(string name)
    {
      Name = name;
    }

    public override string ToString()
    {
      return Name.ToString();
    }

    private Doctor() // required for EF
    {

    }
  }
}
