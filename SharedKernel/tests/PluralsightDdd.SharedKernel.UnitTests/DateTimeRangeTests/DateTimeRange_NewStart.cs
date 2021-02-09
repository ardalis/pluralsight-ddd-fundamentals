using System;
using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeTests
{
  public class DateTimeRange_NewStart
  {
    [Fact]
    public void ReturnsNewObjectWithGivenEndDate()
    {
      DateTime newStartTime = DateTimes.TestDateTime.AddHours(-1);
      var dtr = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));

      var newDtr = dtr.NewStart(newStartTime);

      dtr.Should().NotBeSameAs(newDtr);
      newDtr.Start.Should().Be(newStartTime);
    }
  }

}
