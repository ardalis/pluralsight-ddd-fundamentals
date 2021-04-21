using System;
using Ardalis.Specification;

namespace FrontDesk.Core.ScheduleAggregate.Specifications
{
  public class ScheduleByIdWithAppointmentsSpec : Specification<Schedule>, ISingleResultSpecification
  {
    public ScheduleByIdWithAppointmentsSpec(Guid scheduleId)
    {
      Query
        .Where(schedule => schedule.Id == scheduleId)
        .Include(schedule => schedule.Appointments); // NOTE: Includes *all* appointments
    }
  }
}
