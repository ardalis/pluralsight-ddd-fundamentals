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
              schedule.Appointments != null &&
              schedule.Appointments.Any(appointment => appointment.TimeRange.Start.Date == date.Date));
    }
  }
}
