using ClinicManagement.Core.ValueObjects;
using PluralsightDdd.SharedKernel;

namespace ClinicManagement.Core.Aggregates
{
  public class Patient : BaseEntity<int>
  {
    public int ClientId { get; private set; }
    public string Name { get; set; }
    public string Sex { get; set; }
    public AnimalType AnimalType { get; set; }
    public int? PreferredDoctorId { get; set; }

    public Patient(int clientId,
      string name, string sex,
      int? preferredDoctorId = null)
    {
      ClientId = clientId;
      Name = name;
      Sex = sex;
      PreferredDoctorId = preferredDoctorId;
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
