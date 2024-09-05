using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.AppointmentType;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.AppointmentTypeEndpoints
{
  [Collection("Sequential")]
  public class List : IClassFixture<CustomWebApplicationFactory<Program>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public List(CustomWebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Returns3AppointmentTypes()
    {
      var result = await _client.GetAndDeserializeAsync<ListAppointmentTypeResponse>(ListAppointmentTypeRequest.Route, _outputHelper);

      Assert.Equal(3, result.AppointmentTypes.Count);
      Assert.Contains(result.AppointmentTypes, x => x.Name == "Wellness Exam");
    }
  }
}
