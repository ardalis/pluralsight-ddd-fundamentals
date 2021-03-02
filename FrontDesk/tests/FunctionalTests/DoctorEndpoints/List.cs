using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Doctor;
using FrontDesk.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.DoctorEndpoints
{
  [Collection("Sequential")]
  public class List : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public List(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Returns3Doctors()
    {
      var result = await _client.GetAndDeserialize<ListDoctorResponse>(ListDoctorRequest.Route, _outputHelper);

      Assert.Equal(3, result.Doctors.Count);
      Assert.Contains(result.Doctors, x => x.Name == "Dr. Smith");
    }
  }
}
