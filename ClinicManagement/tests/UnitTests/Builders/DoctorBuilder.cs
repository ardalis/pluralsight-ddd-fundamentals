using ClinicManagement.Core.Aggregates;

namespace UnitTests.Builders
{
  public class DoctorBuilder
  {
    private Doctor _doctor;

    public DoctorBuilder()
    {
      WithDefaultValues();
    }

    public DoctorBuilder Id(int id)
    {
      _doctor.Id = id;
      return this;
    }

    public DoctorBuilder SetDoctor(Doctor doctor)
    {
      _doctor = doctor;
      return this;
    }

    public DoctorBuilder WithDefaultValues()
    {
      _doctor = new Doctor(1, "Test Doctor");

      return this;
    }

    public Doctor Build() => _doctor;
  }
}
