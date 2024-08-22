namespace ClinicManagement.Contracts;

public record ClientUpdatedEvent(int Id, string Name)
{
  public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}
