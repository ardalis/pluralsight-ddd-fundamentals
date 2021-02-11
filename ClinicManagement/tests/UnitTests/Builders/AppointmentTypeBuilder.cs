using ClinicManagement.Core.Aggregates;

namespace UnitTests.Builders
{
  public class AppointmentTypeBuilder
  {
    private AppointmentType _appointmentType;

    public AppointmentTypeBuilder()
    {
      WithDefaultValues();
    }

    public AppointmentTypeBuilder Id(int id)
    {
      _appointmentType.Id = id;
      return this;
    }

    public AppointmentTypeBuilder SetAppointmentType(AppointmentType appointmentType)
    {
      _appointmentType = appointmentType;
      return this;
    }

    public AppointmentTypeBuilder WithDefaultValues()
    {
      _appointmentType = new AppointmentType(1, "Test AppointmentType", "Test Code", 30);

      return this;
    }

    public AppointmentType Build() => _appointmentType;
  }
}
