using System;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeTests
{
  public class DateTimeRange_Constructor
  {
    [Fact]
    public void ThrowsGivenStartAfterEnd()
    {
      var ex = Assert.Throws<ArgumentException>(() =>
          new DateTimeRange(DateTimes.TestDateTime, DateTimes.TestDateTime.AddDays(-1)));
    }

    [Fact]
    public void ThrowsGivenStartAndNegativeTimeSpan()
    {
      TimeSpan span = new TimeSpan(-1, 0, 0);
      var ex = Assert.Throws<ArgumentException>(() =>
          new DateTimeRange(DateTimes.TestDateTime, span));
    }
  }
}
