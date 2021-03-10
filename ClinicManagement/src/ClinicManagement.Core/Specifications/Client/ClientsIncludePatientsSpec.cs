using Ardalis.Specification;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Core.Specifications
{
  public class ClientsIncludePatientsSpec : Specification<Client>
  {
    public ClientsIncludePatientsSpec()
    {
      Query
        .Include(client => client.Patients)
        .OrderBy(client => client.FullName);
    }
  }
}
