using System;

namespace PluralsightDdd.SharedKernel.Interfaces
{
  public interface IDomainEvent
  {
    /// <summary>
    /// UTC DateTime when event occurred
    /// </summary>
    DateTime DateTimeEventOccurred { get; }
  }
}
