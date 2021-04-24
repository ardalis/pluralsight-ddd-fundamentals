using Ardalis.Specification;

namespace FrontDesk.Core.SyncedAggregates.Specifications
{
  public class ClientByIdIncludePatientsSpecification : Specification<Client>, ISingleResultSpecification
  {
    public ClientByIdIncludePatientsSpecification(int clientId)
    {
      Query
        .Include(client => client.Patients)
        .Where(client => client.Id == clientId);
    }
  }
}
