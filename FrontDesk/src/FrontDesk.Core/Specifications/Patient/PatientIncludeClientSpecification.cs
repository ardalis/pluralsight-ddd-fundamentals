using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
{
  public class PatientIncludeClientSpecification : Specification<Patient>
  {
    public PatientIncludeClientSpecification()
    {
      Query.Include(nameof(Patient.Client));

      Query.OrderBy(patient => patient.Name);
    }
  }
}
