using System.Collections.Generic;

namespace PluralsightDdd.SharedKernel
{
  /// <summary>
  /// Base types for all Entities which track state using a given Id.
  /// </summary>
  public abstract class BaseEntity<TId>
  {
    public TId Id { get; set; }

    public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
  }
}
