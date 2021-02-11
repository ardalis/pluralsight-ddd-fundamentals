using Ardalis.Specification;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Core.Specifications
{
  public class ClientsIncludePatientsSpecification : Specification<Client>
  {
    public ClientsIncludePatientsSpecification()
    {
      Query
        .Include(client => client.Patients)
        .OrderBy(client => client.FullName);
    }
  }
}
