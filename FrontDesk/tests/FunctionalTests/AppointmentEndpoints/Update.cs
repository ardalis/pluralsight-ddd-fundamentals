using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
  public class Update : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;
    private int _testAppointmentTypeId = 1;
    private int _testRoomId = 2;
    private int _testDoctorId = 3;
    private string _testTitle = "updated title";

    public Update(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task UpdatesAnExistingAppointment()
    {
      var scheduleRoute = ListScheduleRequest.Route;
      var schedule = (await _client.GetAndDeserialize<ListScheduleResponse>(scheduleRoute)).Schedules.First();

      string route = ListAppointmentRequest.Route.Replace("{ScheduleId}", schedule.Id.ToString());

      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(route, _outputHelper);

      var firstAppt = result.Appointments.First();

      var jsonContent = GetUpdatedAppointmentJson(firstAppt);

      var rawResult = await _client.PutAsync(UpdateAppointmentRequest.Route, jsonContent);
      rawResult.EnsureSuccessStatusCode();
      var stringResponse = await rawResult.Content.ReadAsStringAsync();
      var endResult = stringResponse.FromJson<UpdateAppointmentResponse>();

      Assert.Equal(firstAppt.AppointmentId, endResult.Appointment.AppointmentId);
      Assert.Equal(_testAppointmentTypeId, endResult.Appointment.AppointmentTypeId);
      Assert.Equal(_testRoomId, endResult.Appointment.RoomId);
      Assert.Equal(_testDoctorId, endResult.Appointment.DoctorId);
      Assert.Equal(_testTitle, endResult.Appointment.Title);
    }

    private StringContent GetUpdatedAppointmentJson(AppointmentDto originalAppointment)
    {
      var request = new UpdateAppointmentRequest()
      {
        // requiredkeys
        Id = originalAppointment.AppointmentId,
        ScheduleId = originalAppointment.ScheduleId,

        // fields to update
        AppointmentTypeId = _testAppointmentTypeId,
        RoomId = _testRoomId,
        Title = _testTitle,
        DoctorId = _testDoctorId
      };
      var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

      return jsonContent;
    }
  }
}
