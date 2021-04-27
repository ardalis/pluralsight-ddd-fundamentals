namespace PluralsightDdd.SharedKernel.Interfaces
{
  /// <summary>
  /// Integration events are used to communicate between bounded contexts and/or applications.
  /// They are often mapped from domain events in the notifying system 
  /// and sometimes to domain events in the consuming system
  /// </summary>
  public interface IIntegrationEvent
  {
    string EventType { get; }
  }
}
