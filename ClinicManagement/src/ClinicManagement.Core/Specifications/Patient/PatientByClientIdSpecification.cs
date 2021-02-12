using Ardalis.Specification;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Core.Specifications
{
  public class PatientByClientIdSpecification : Specification<Patient>
  {
    public PatientByClientIdSpecification(int clientId)
    {
      Query
          .Where(patient => patient.ClientId == clientId);

      Query.OrderBy(patient => patient.Name);
    }
  }
}
