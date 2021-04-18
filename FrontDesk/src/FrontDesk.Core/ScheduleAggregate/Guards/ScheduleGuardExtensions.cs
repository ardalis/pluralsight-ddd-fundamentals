using System.Collections.Generic;
using System.Linq;
using FrontDesk.Core.Exceptions;
using FrontDesk.Core.ScheduleAggregate;

namespace Ardalis.GuardClauses
{
  public static class ScheduleGuardExtensions
  {
    public static void DuplicateAppointment(this IGuardClause guardClause, IEnumerable<Appointment> existingAppointments, Appointment newAppointment, string parameterName)
    {
      if (existingAppointments.Any(a => a.Id == newAppointment.Id))
      {
        throw new DuplicateAppointmentException("Cannot add duplicate appointment to schedule.", parameterName);
      }
    }
  }
}
