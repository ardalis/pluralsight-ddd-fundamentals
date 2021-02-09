using System;
using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeTests
{
  public class DateTimeRange_NewDuration
  {
    [Fact]
    public void ReturnsNewObjectWithGivenDuration()
    {
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));

      var newDtr = dtr.NewDuration(TimeSpan.FromHours(2));

      dtr.Should().NotBeSameAs(newDtr);
      newDtr.DurationInMinutes().Should().Be(120);
    }
  }

}
