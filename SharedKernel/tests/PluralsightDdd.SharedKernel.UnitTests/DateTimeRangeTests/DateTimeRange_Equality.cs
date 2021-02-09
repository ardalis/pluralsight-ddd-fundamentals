using System;
using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.DateTimeRangeTests
{
  public class DateTimeRange_Equality
  {
    [Fact]
    public void ReturnsTrueGivenSameStartEnd()
    {
      var dtr1 = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));
      var dtr2 = new DateTimeRange(DateTimes.TestDateTime, TimeSpan.FromHours(1));

      dtr1.Should().NotBeSameAs(dtr2);
      dtr1.Should().Equals(dtr2);
      dtr2.Should().Equals(dtr1);
      (dtr1 == dtr2).Should().BeTrue();
    }
  }

}
