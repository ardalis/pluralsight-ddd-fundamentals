using PluralsightDdd.SharedKernel;

namespace ClinicManagement.Api.ApplicationEvents
{
  public class NamedEntityUpdatedEvent : BaseIntegrationEvent
  {
    public string EventType { get; set; }
    public NamedEntity Entity { get; set; }

    public NamedEntityUpdatedEvent(NamedEntity entity, string eventType)
    {
      Entity = entity;
      EventType = eventType;
    }
  }
}
