using System;
using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeTests
{
  public class DateTimeRange_Overlaps
  {
    [Fact]
    public void ReturnsTrueGivenSameDateTimeRange()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));

      var result = dtr.Overlaps(dtr);

      result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrueGivenEarlierRangeExceedingStart()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));
      var earlier = new DateTimeRange(DateTimes.TestDateTime.AddMinutes(-5), TimeSpan.FromMinutes(10));

      var result = dtr.Overlaps(earlier);

      result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrueGivenRangeWithinRange()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));
      var within = new DateTimeRange(DateTimes.TestDateTime.AddMinutes(5), TimeSpan.FromMinutes(10));

      var result = dtr.Overlaps(within);

      result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrueGivenRangeStartingWithinRangeEndingLater()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));
      var endslater = new DateTimeRange(dtr.End.AddMinutes(-5), TimeSpan.FromMinutes(10));

      var result = dtr.Overlaps(endslater);

      result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalseGivenRangeEndingBeforeStart()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));
      var endsearlier = new DateTimeRange(dtr.Start.AddHours(-1), TimeSpan.FromMinutes(10));

      var result = dtr.Overlaps(endsearlier);

      result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalseGivenRangeStartingAfterEnd()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));
      var startslater = new DateTimeRange(dtr.End.AddMinutes(1), TimeSpan.FromMinutes(10));

      var result = dtr.Overlaps(startslater);

      result.Should().BeFalse();
    }

  }
}
