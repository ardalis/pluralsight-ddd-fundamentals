using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Core.Aggregates
{
  public class AppointmentType : BaseEntity<int>, IAggregateRoot
  {
    public string Name { get; private set; }
    public string Code { get; private set; }
    public int Duration { get; private set; }

    public AppointmentType(int id, string name, string code, int duration)
    {
      Id = id;
      Name = name;
      Code = code;
      Duration = duration;
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
