using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShared.Models.AppointmentType;
using FrontDesk.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.Api
{
  public class AppointmentTypesList : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public AppointmentTypesList(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Returns3AppointmentTypes()
    {
      var response = await _client.GetAsync("/api/appointment-types");
      response.EnsureSuccessStatusCode();
      var stringResponse = await response.Content.ReadAsStringAsync();
      _outputHelper.WriteLine(stringResponse);
      var result = JsonSerializer.Deserialize<ListAppointmentTypeResponse>(stringResponse,
        Constants.DefaultJsonOptions);

      Assert.Equal(3, result.AppointmentTypes.Count());
      Assert.Contains(result.AppointmentTypes, x => x.Name == "Wellness Exam");
    }
  }
}
