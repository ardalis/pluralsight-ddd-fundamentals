using FrontDesk.Core.SyncedAggregates;

namespace UnitTests.Builders
{
  public class DoctorBuilder
  {
    public const string DEFAULT_NAME = "Test Doctor";
    private Doctor _doctor;
    private int _id;
    private string _name = DEFAULT_NAME;

    public DoctorBuilder()
    {
    }

    public DoctorBuilder WithId(int id)
    {
      _id = id;
      return this;
    }
    public DoctorBuilder WithName(string name)
    {
      _name = name;
      return this;
    }

    public DoctorBuilder WithDefaultValues()
    {
      _doctor = new Doctor(0, DEFAULT_NAME);

      return this;
    }

    public Doctor Build() => new Doctor(_id, _name);
  }
}
