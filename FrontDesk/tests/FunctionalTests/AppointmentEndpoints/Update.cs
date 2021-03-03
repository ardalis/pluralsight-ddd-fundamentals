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
  public class Update : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;
    private int _testAppointmentTypeId = 1;
    private int _testClientId = 1;
    private int _testPatientId = 1;
    private int _testRoomId = 2;
    private int _testDoctorId = 1;

    public Update(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task UpdatesAnExistingAppointment()
    {
      var result = await _client.GetAndDeserialize<ListAppointmentResponse>(ListAppointmentRequest.Route, _outputHelper);

      var firstAppt = result.Appointments.Single();

      var jsonContent = GetUpdatedAppointmentJson(firstAppt);

      var rawResult = await _client.PutAsync(UpdateAppointmentRequest.Route, jsonContent);
      rawResult.EnsureSuccessStatusCode();
      var stringResponse = await rawResult.Content.ReadAsStringAsync();
      var endResult = stringResponse.FromJson<UpdateAppointmentResponse>();

      Assert.Equal(firstAppt.AppointmentId, endResult.Appointment.AppointmentId);
      Assert.Equal(_testAppointmentTypeId, endResult.Appointment.AppointmentTypeId);
      Assert.Equal(_testRoomId, endResult.Appointment.RoomId);

      // TODO: Any other fields need updated? Remove unused ones from UpdateAppointmentRequest
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
        RoomId = _testRoomId

        // things we shouldn't update with this method
        //ClientId = originalAppointment.ClientId,
        //PatientId = originalAppointment.PatientId,
      };
      var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

      return jsonContent;
    }
  }
}
