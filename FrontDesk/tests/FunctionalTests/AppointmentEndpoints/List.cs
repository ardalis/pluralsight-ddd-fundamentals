using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Appointment;
using FrontDesk.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.AppointmentEndpoints
{
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
    public async Task Returns1AppointmentForDarwin()
    {
      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(ListAppointmentRequest.Route, _outputHelper);

      var firstAppt = result.Appointments.Single();
      _outputHelper.WriteLine(firstAppt.ToString());

      Assert.Contains("Darwin", firstAppt.Title);
      Assert.Equal(1, firstAppt.RoomId);
    }
  }
}
