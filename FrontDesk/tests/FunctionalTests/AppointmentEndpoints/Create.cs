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
  public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;
    private int _testAppointmentTypeId = 1;
    private int _testClientId = 1;
    private int _testPatientId = 1;
    private int _testRoomId = 1;
    private int _testDoctorId = 1;

    public Create(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task CreatesANewAppointment()
    {
      // get schedule
      var listResult = await _client.GetAndDeserialize<ListScheduleResponse>(ListScheduleRequest.Route, _outputHelper);
      var schedule = listResult.Schedules.First();
      string scheduleId = schedule.Id.ToString();

      string listRoute = ListAppointmentRequest.Route.Replace("{ScheduleId}", scheduleId);

      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(listRoute, _outputHelper);

      var jsonContent = GetValidNewAppointmentJson(schedule.Id);

      string createRoute = CreateAppointmentRequest.Route.Replace("{ScheduleId}", scheduleId);
      var rawResult = await _client.PostAsync(createRoute, jsonContent);
      rawResult.EnsureSuccessStatusCode();
      var stringResponse = await rawResult.Content.ReadAsStringAsync();
      var endResult = stringResponse.FromJson<CreateAppointmentResponse>();

      Assert.Equal(schedule.Id, endResult.Appointment.ScheduleId);
    }

    private StringContent GetValidNewAppointmentJson(Guid scheduleId)
    {
      var request = new CreateAppointmentRequest()
      {
        AppointmentTypeId = _testAppointmentTypeId,
        ClientId = _testClientId,
        DateOfAppointment = new OfficeSettings().TestDate,
        Title = "new appointment title",
        PatientId = _testPatientId,
        RoomId = _testRoomId,
        ScheduleId = scheduleId,
        SelectedDoctor = _testDoctorId
      };
      var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

      return jsonContent;
    }
  }
}
