using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace UnitTests.Builders
{
  public class ScheduleBuilder
  {
    private Guid _scheduleId;
    private int _clinicId;
    public const int TEST_CLINIC_ID = 2;

    public ScheduleBuilder WithId(Guid id)
    {
      _scheduleId = id;
      return this;
    }

    public ScheduleBuilder WithDefaultValues()
      {
      _scheduleId = Guid.NewGuid();
      _clinicId = TEST_CLINIC_ID;
      return this;
      }

    public Schedule Build()
    {
      return new Schedule(_scheduleId, new DateTimeOffsetRange(DateTimeOffset.Now.Date, TimeSpan.FromDays(1)), _clinicId);
    }
  }
}
