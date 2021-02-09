using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeTests
{
  public class DateTimeRange_CreateOneWeekRange
  {
    [Fact]
    public void CreatesRangeWithStartDateLastingSevenDay()
    {
      var dtr = DateTimeRange.CreateOneWeekRange(DateTimes.TestDateTime);

      dtr.Start.Should().Be(DateTimes.TestDateTime);
      dtr.End.Should().Be(dtr.Start.AddDays(7));
    }
  }
}
