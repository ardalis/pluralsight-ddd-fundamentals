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
  public class GetById : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public GetById(CustomWebApplicationFactory<Startup> factory,
      ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task ReturnsClientGivenValidId ()
    {
      string route = GetByIdClientRequest.Route.Replace("{ClientId}", "1");
      var result = await _client.GetAndDeserialize<GetByIdClientResponse>(route, _outputHelper);

      Assert.Equal("Steve Smith", result.Client.FullName);
    }

    [Fact]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
      string route = GetByIdClientRequest.Route.Replace("{ClientId}", "0");
      _ = await _client.GetAndEnsureNotFound(route, _outputHelper);
    }
  }
}
