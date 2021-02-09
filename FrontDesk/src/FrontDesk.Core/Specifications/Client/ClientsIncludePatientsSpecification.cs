using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
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
