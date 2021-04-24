using FrontDesk.Core.SyncedAggregates;

namespace UnitTests.Builders
{
  public class ClientBuilder
  {
    public const string DEFAULT_FULL_NAME = "Steve Smith";
    private Client _client;
    private string _fullname = DEFAULT_FULL_NAME;

    public ClientBuilder()
    {
      WithDefaultValues();
    }

    public ClientBuilder WithFullname(string fullname)
    {
      _fullname = fullname;
      return this;
    }

    public ClientBuilder SetClient(Client client)
    {
      _client = client;
      return this;
    }

    public ClientBuilder WithDefaultValues()
    {
      _client = new Client(DEFAULT_FULL_NAME, "Test Preferred", "Test Salutation", 1, "test@test.com");

      return this;
    }

    public Client Build() => new Client(_fullname, "Test Preferred", "Test Salutation", 1, "test@test.com");
  }
}
