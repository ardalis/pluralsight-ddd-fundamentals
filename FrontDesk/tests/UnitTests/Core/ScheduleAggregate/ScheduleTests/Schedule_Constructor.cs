using System;
using System.Collections.Generic;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;
using Xunit;

namespace UnitTests.Core.AggregatesEntities.ScheduleTests
{
  public class Schedule_Constructor
  {
    private readonly Guid _scheduleId = Guid.Parse("4a17e702-c20e-4b87-b95b-f915c5a794f7");
    private readonly DateTime _startTime = new DateTime(2021, 01, 01, 10, 00, 00);
    private readonly DateTime _endTime = new DateTime(2021, 01, 01, 11, 00, 00);
    private readonly DateTimeOffsetRange _dateRange;
    private readonly int _clinicId = 1;

    public Schedule_Constructor()
    {
      _dateRange = new DateTimeOffsetRange(_startTime, _endTime);
    }

    [Fact]
    public void CreateConstructor()
    {
      var appointments = new List<Appointment>();

      var schedule = new Schedule(_scheduleId, _dateRange, _clinicId);

      Assert.Equal(_scheduleId, schedule.Id);
      Assert.Equal(_startTime, schedule.DateRange.Start);
      Assert.Equal(_endTime, schedule.DateRange.End);
      Assert.Equal(_clinicId, schedule.ClinicId);
    }
  }
}
