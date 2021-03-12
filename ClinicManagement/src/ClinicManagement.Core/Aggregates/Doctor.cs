using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Core.Aggregates
{
  public class Doctor : BaseEntity<int>, IAggregateRoot
  {
    public string Name { get; set; }

    public Doctor(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
