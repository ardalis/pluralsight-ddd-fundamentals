using Ardalis.Specification;
using FrontDesk.Core.SyncedAggregates;

namespace FrontDesk.Core.SyncedAggregates.Specifications
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
