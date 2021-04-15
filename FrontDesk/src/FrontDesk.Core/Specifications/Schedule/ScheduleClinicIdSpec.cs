using Ardalis.Specification;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Specifications
{
  public class ScheduleClinicIdSpec : Specification<Schedule>
  {
    public ScheduleClinicIdSpec(int clinicId)
    {
      Query
          .Where(schedule =>
              schedule.ClinicId == clinicId);
    }
  }
}
