using Ardalis.Specification;
using FrontDesk.Core.SyncedAggregates;

namespace FrontDesk.Core.SyncedAggregates.Specifications
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
