using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
{
  public class PatientByClientIdSpecification : Specification<Patient>
  {
    public PatientByClientIdSpecification(int clientId)
    {
      Query
          .Include(nameof(Patient.Client))
          .Where(patient => patient.ClientId == clientId);

      Query.OrderBy(patient => patient.Name);
    }
  }
}
