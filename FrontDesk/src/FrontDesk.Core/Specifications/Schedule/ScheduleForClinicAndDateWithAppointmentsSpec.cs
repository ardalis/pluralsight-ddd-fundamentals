using System;
using System.Linq;
using Ardalis.Specification;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Specifications
{
  public class ScheduleForClinicAndDateWithAppointmentsSpec : Specification<Schedule>, ISingleResultSpecification
  {
    public ScheduleForClinicAndDateWithAppointmentsSpec(int clinicId, DateTimeOffset date)
    {
      var endDate = date.AddDays(1);
      Query
          .Where(schedule =>
            schedule.ClinicId == clinicId &&
            schedule.Appointments != null)
          .Include(s => s.Appointments.Where(a => a.TimeRange.Start > date && a.TimeRange.End < endDate));

      // See: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/whatsnew#filtered-include

      // TODO: Only include appointments for the specified date
      // NOTE: This worked when using DateTime, but EF Core has 
      // issues with DateTimeOffset in queries
    }
  }
}
