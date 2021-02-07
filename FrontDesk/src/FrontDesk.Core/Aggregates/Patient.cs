using FrontDesk.Core.ValueObjects;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Patient : BaseEntity<int>, IAggregateRoot
  {
    public int ClientId { get; private set; }
    public string Name { get; private set; }
    public string Sex { get; private set; }
    public virtual AnimalType AnimalType { get; private set; }
    public int? PreferredDoctorId { get; private set; }

    private Client _client;
    public Client Client
    {
      get
      {
        return _client;
      }
      private set
      {
        _client = value;
      }
    }


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
