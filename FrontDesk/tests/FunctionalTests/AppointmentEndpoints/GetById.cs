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
  [Collection("Sequential")]
  public class GetById : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public GetById(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task GetsExistingAppointment()
    {
      var listResponse = await _client.GetAndDeserialize<ListAppointmentResponse>(ListAppointmentRequest.Route, _outputHelper);

      var firstAppt = listResponse.Appointments.First();
      _outputHelper.WriteLine(firstAppt.ToString());

      string route = GetByIdAppointmentRequest.Route.Replace("{AppointmentId}", firstAppt.AppointmentId.ToString());
      route = route.Replace("{ScheduleId}", firstAppt.ScheduleId.ToString());
      var result = await _client.GetAndDeserialize<GetByIdAppointmentResponse>(route, _outputHelper);

      Assert.Equal(firstAppt.PatientId, result.Appointment.PatientId);
      Assert.Equal(firstAppt.PatientName, result.Appointment.PatientName);
      Assert.Equal(firstAppt.RoomId, result.Appointment.RoomId);
      Assert.Equal(firstAppt.ToString(), result.Appointment.ToString());
    }
  }
}
