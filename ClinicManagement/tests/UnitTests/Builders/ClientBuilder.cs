using ClinicManagement.Core.Aggregates;

namespace UnitTests.Builders
{
  public class ClientBuilder
  {
    private Client _client;

    public ClientBuilder()
    {
      WithDefaultValues();
    }

    public ClientBuilder Id(int id)
    {
      _client.Id = id;
      return this;
    }

    public ClientBuilder SetClient(Client client)
    {
      _client = client;
      return this;
    }

    public ClientBuilder WithDefaultValues()
    {
      _client = new Client("Test Client", "Test Preferred", "Test Salutation", 1, "test@test.com");

      return this;
    }

    public Client Build() => _client;
  }
}
