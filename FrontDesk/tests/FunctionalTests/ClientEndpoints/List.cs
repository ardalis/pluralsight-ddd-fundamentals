using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Client;
using FrontDesk.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.ClientEndpoints
{
  [Collection("Sequential")]
  public class List : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public List(CustomWebApplicationFactory<Startup> factory,
      ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task ReturnsManyClients()
    {
      var result = await _client.GetAndDeserialize<ListClientResponse>(ListClientRequest.Route, _outputHelper);

      Assert.True(result.Clients.Count > 10);
      Assert.Contains(result.Clients, x => x.FullName == "Steve Smith");
    }

    [Fact]
    public async Task IncludesPatientIds()
    {
      var result = await _client.GetAndDeserialize<ListClientResponse>(ListClientRequest.Route, _outputHelper);

      Assert.NotEmpty(result.Clients.First().Patients);
    }
  }
}
