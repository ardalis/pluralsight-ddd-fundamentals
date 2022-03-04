using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Api.ApplicationEvents
{
  public class NamedEntityCreatedEvent : IApplicationEvent
  {
    public string EventType { get; set; }
    public NamedEntity Entity { get; set; }

    public NamedEntityCreatedEvent(NamedEntity entity, string eventType)
    {
      Entity = entity;
      EventType = eventType;
    }
  }
}
