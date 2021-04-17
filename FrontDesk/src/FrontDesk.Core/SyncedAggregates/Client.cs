using System.Collections.Generic;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.SyncedAggregates
{
  public class Client : BaseEntity<int>, IAggregateRoot
  {
    public Client(int id)
    {
      Id = id;
      Patients = new List<Patient>();
    }

    public Client(string fullName,
      string preferredName,
      string salutation,
      int preferredDoctorId,
      string emailAddress)
    {
      FullName = fullName;
      PreferredName = preferredName;
      Salutation = salutation;
      PreferredDoctorId = preferredDoctorId;
      EmailAddress = emailAddress;
    }

    //private Client() //required for EF
    //{
    //}

    public string FullName { get; private set; }
    public string PreferredName { get; private set; }
    public string Salutation { get; private set; }
    public string EmailAddress { get; private set; }
    public int PreferredDoctorId { get; private set; }
    public IList<Patient> Patients { get; private set; } = new List<Patient>();

    public override string ToString()
    {
      return FullName.ToString();
    }
  }
}
