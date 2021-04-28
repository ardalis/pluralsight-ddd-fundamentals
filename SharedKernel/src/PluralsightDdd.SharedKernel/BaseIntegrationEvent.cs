using System;
using MediatR;

namespace PluralsightDdd.SharedKernel
{
  /// <summary>
  /// Integration events are used to communicate between bounded contexts and/or applications.
  /// They are often mapped from domain events in the notifying system 
  /// and sometimes to domain events in the consuming system
  /// </summary>
  public abstract class BaseIntegrationEvent : INotification
  {
    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
    string EventType { get; }

  }
}
