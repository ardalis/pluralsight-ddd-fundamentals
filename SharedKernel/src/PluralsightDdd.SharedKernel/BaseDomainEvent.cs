using System;
using MediatR;

namespace PluralsightDdd.SharedKernel
{
  public abstract class BaseDomainEvent : INotification
  {
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
  }
}
