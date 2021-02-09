using System;
using FluentAssertions;
using Xunit;

namespace PluralsightDdd.SharedKernel.UnitTests.BaseDomainEventTests
{
  public class BaseDomainEvent_Constructor
  {
    public class TestEvent : BaseDomainEvent
    { }

    [Fact]
    public void SetsTimeToCurrentTime()
    {
      var newEvent = new TestEvent();

      newEvent.DateOccurred.Should().BeCloseTo(DateTime.UtcNow, 100);
    }
  }
}
