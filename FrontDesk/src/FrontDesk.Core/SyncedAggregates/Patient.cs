using FrontDesk.Core.ValueObjects;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.SyncedAggregates
{
  public class Patient : BaseEntity<int>
  {
    public Patient(int clientId, string name, string sex, AnimalType animalType, int? preferredDoctorId = null)
    {
      ClientId = clientId;
      Name = name;
      Sex = sex;
      AnimalType = animalType;
      PreferredDoctorId = preferredDoctorId;
    }

    public Patient(int id)
    {
      Id = id;
    }

    private Patient() // required for EF
    {
    }

    public int ClientId { get; private set; }
    public string Name { get; private set; }
    public string Sex { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public int? PreferredDoctorId { get; private set; }

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
