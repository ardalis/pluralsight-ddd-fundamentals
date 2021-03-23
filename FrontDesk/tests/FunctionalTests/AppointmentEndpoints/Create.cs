using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Appointment;
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
      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(ListAppointmentRequest.Route, _outputHelper);

      var firstAppt = result.Appointments.First();
      Guid scheduleId = firstAppt.ScheduleId;

      var jsonContent = GetValidNewAppointmentJson(scheduleId);

      var rawResult = await _client.PostAsync(CreateAppointmentRequest.Route, jsonContent);
      rawResult.EnsureSuccessStatusCode();
      var stringResponse = await rawResult.Content.ReadAsStringAsync();
      var endResult = stringResponse.FromJson<CreateAppointmentResponse>();

      Assert.Equal(scheduleId, endResult.Appointment.ScheduleId);
    }

    private StringContent GetValidNewAppointmentJson(Guid scheduleId)
    {
      var request = new CreateAppointmentRequest()
      {
        AppointmentTypeId = _testAppointmentTypeId,
        ClientId = _testClientId,
        DateOfAppointment = new OfficeSettings().TestDate,
        Details = "new appointment details",
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
