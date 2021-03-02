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
      route = route.Replace("{scheduleId}", firstAppt.ScheduleId.ToString());
      var deleteResponse = await _client.DeleteAsync(route);
      deleteResponse.EnsureSuccessStatusCode();
      
      result = await _client.GetAndDeserialize<ListAppointmentResponse>(ListAppointmentRequest.Route, _outputHelper);

      Assert.Empty(result.Appointments);
    }
  }
}
