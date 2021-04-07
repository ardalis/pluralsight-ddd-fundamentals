using System;
using System.Linq;
using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
{
  public class ScheduleForClinicAndDateWithAppointmentsSpec : Specification<Schedule>, ISingleResultSpecification
  {
    public ScheduleForClinicAndDateWithAppointmentsSpec(int clinicId, DateTimeOffset date)
    {
      Query
          .Include(nameof(Schedule.Appointments))
          .Where(schedule =>
              schedule.ClinicId == clinicId &&
              schedule.Appointments != null);
      // TODO: Only include appointments for the specified date
      // NOTE: This worked when using DateTime, but EF Core has 
      // issues with DateTimeOffset in queries
    }
  }
}
