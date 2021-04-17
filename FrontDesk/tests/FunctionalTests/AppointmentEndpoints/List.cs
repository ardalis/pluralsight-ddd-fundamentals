using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Appointment;
using BlazorShared.Models.Schedule;
using FrontDesk.Api;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.AppointmentEndpoints
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
    public async Task ReturnsAppointmentsIncludingOneForDarwin()
    {
      // get schedule
      var listResult = await _client.GetAndDeserialize<ListScheduleResponse>(ListScheduleRequest.Route, _outputHelper);
      Assert.True(listResult.Schedules.Count == 1);
      var schedule = listResult.Schedules.First();
      _outputHelper.WriteLine($"Schedule: {schedule}");

      string route = ListAppointmentRequest.Route.Replace("{ScheduleId}", schedule.Id.ToString());

      _outputHelper.WriteLine($"Route: {route}");

      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(route, _outputHelper);

      var darwinAppt = result.Appointments.First(a => a.Title.Contains("Darwin"));
      _outputHelper.WriteLine(darwinAppt.ToString());

      Assert.Contains("Darwin", darwinAppt.Title);
      Assert.Equal(1, darwinAppt.RoomId);
    }
  }
}
