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
  public class Delete : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public Delete(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task DeletesExistingAppointment()
    {
      // get existing appointment
      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(ListAppointmentRequest.Route, _outputHelper);

      var firstAppt = result.Appointments.Single();
      _outputHelper.WriteLine(firstAppt.ToString());

      // delete it
      string route = DeleteAppointmentRequest.Route.Replace("{appointmentId}", firstAppt.AppointmentId.ToString());
      var scheduleId = firstAppt.ScheduleId.ToString();

      route = route.Replace("{scheduleId}", scheduleId);
      var deleteResponse = await _client.DeleteAsync(route);
      deleteResponse.EnsureSuccessStatusCode();

      var response = await _client.GetAndDeserialize<GetByIdScheduleResponse>(GetByIdScheduleRequest.Route.Replace("{scheduleId}", scheduleId), _outputHelper);

      Assert.Empty(response.Schedule.AppointmentIds);
    }
  }
}
