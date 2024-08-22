namespace ClinicManagement.Contracts;

public record DoctorCreatedEvent(int Id, string Name)
{
  public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}
