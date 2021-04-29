using System;
using MediatR;

namespace PluralsightDdd.SharedKernel
{
  public abstract class BaseDomainEvent : INotification
  {
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
  }
}
