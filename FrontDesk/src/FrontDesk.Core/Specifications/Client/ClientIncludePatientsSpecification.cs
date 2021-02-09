using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
{
  public class ClientByIdIncludePatientsSpecification : Specification<Client>
  {
    public ClientByIdIncludePatientsSpecification(int clientId)
    {
      Query
        .Include(client => client.Patients)
        .Where(client => client.Id == clientId)
        .OrderBy(client => client.FullName);
    }
  }
}
