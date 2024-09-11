using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Client;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.Api
{
  [Collection("Sequential")]
  public class ClientsList : IClassFixture<CustomWebApplicationFactory<Program>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public ClientsList(CustomWebApplicationFactory<Program> factory,
      ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Returns2Clients()
    {
      var result = await _client.GetAndDeserializeAsync<ListClientResponse>("/api/clients", _outputHelper);

      Assert.Equal(13, result.Clients.Count());
      Assert.Contains(result.Clients, x => x.FullName == "Steve Smith");
    }

    [Fact]
    public async Task IncludesPatientIds()
    {
      var result = await _client.GetAndDeserializeAsync<ListClientResponse>("/api/clients", _outputHelper);

      Assert.NotEmpty(result.Clients.First().Patients);
    }
  }
}
