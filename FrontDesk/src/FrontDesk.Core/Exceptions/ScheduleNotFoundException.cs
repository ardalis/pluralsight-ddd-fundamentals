using System;

namespace FrontDesk.Core.Exceptions
{
  public class ScheduleNotFoundException : Exception
  {
    public ScheduleNotFoundException(string message) : base(message)
    {
    }

    public ScheduleNotFoundException(Guid scheduleId) : base($"No schedule with id {scheduleId} found.")
    {
    }
  }
}
