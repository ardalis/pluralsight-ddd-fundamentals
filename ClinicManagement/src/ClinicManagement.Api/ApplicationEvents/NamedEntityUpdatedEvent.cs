using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Api.ApplicationEvents
{
  public class NamedEntityUpdatedEvent : IApplicationEvent
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
