namespace ClinicManagement.Contracts;

public record DoctorCreatedIntegrationEvent(int Id, string Name)
{
  public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}
