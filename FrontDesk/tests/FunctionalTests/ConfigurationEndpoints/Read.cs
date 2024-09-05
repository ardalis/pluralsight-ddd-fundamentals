using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.ConfigurationEndpoints
{
  [Collection("Sequential")]
  public class Read : IClassFixture<CustomWebApplicationFactory<Program>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public Read(CustomWebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task ReturnsTestDateAsString()
    {
      var result = await _client.GetAndReturnStringAsync(GetConfigurationRequest.Route, _outputHelper);

      var parsed = DateTimeOffset.TryParse(result, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
      Assert.True(parsed);
      Assert.NotEqual(default, date);
    }
  }
}
