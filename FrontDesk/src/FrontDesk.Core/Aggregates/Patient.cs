using FrontDesk.Core.ValueObjects;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Patient : BaseEntity<int>
  {
    public int ClientId { get; private set; }
    public string Name { get; private set; }
    public string Sex { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public int? PreferredDoctorId { get; private set; }

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

    public void UpdateName(string name)
    {
      Name = name;
    }

    public override string ToString()
    {
      return Name.ToString();
    }

    private Patient() // required for EF
    {

    }
  }
}
