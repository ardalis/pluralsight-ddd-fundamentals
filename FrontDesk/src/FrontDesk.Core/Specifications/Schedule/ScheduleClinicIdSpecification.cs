using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
{
  public class ScheduleClinicIdSpecification : Specification<Schedule>
  {
    public ScheduleClinicIdSpecification(int clinicId)
    {
      Query
          .Where(schedule =>
              schedule.ClinicId == clinicId);

    }
  }
}
