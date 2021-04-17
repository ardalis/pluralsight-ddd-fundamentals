using System;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Interfaces
{
  /// <summary>
  /// Note: This interface is just an example; it's not used in this app.
  /// </summary>
  public interface IScheduleRepository
  {
    Schedule GetScheduleForDate(int clinicId, DateTime date);
    void Update(Schedule schedule);
  }
}
