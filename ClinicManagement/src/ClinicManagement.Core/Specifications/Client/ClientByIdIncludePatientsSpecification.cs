using Ardalis.Specification;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Core.Specifications
{
  public class ClientByIdIncludePatientsSpecification : Specification<Client>
  {
    public ClientByIdIncludePatientsSpecification(int clientId)
    {
      Query
        .Include(client => client.Patients)
        .Where(client => client.Id == clientId);
    }
  }
}
