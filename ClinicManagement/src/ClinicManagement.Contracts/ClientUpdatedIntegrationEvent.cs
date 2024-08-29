namespace ClinicManagement.Contracts;

public record ClientUpdatedIntegrationEvent(int Id, string Name)
{
  public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}
