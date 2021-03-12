using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Api.ApplicationEvents
{
  public class EntityCreatedEvent : IApplicationEvent
  {
    public string EventType => "Doctor-Created";
    public NamedEntity Entity { get; set; }

    public EntityCreatedEvent(NamedEntity entity)
    {
      Entity = entity;
    }
  }
}
