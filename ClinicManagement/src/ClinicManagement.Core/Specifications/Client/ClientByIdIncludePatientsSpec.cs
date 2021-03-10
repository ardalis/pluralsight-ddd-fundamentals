using Ardalis.Specification;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Core.Specifications
{
  public class ClientByIdIncludePatientsSpec : Specification<Client>, ISingleResultSpecification
  {
    public ClientByIdIncludePatientsSpec(int clientId)
    {
      Query
        .Include(client => client.Patients)
        .Where(client => client.Id == clientId);
    }
  }
}
