using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class AppointmentType : BaseEntity<int>, IAggregateRoot
  {
    public string Name { get; private set; }
    public string Code { get; private set; }
    public int Duration { get; private set; }

    public AppointmentType(string name, string code, int duration)
    {
      Name = name;
      Code = code;
      Duration = duration;
    }

    public AppointmentType(int id)
    {
      Id = id;
    }

    private AppointmentType() // required for EF
    {
    }

    public override string ToString()
    {
      return Name;
    }

  }
}
