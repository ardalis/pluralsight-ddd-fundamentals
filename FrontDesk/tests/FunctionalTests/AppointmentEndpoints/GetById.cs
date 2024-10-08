﻿using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using BlazorShared.Models.Appointment;
using BlazorShared.Models.Schedule;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.AppointmentEndpoints
{
  [Collection("Sequential")]
  public class GetById : IClassFixture<CustomWebApplicationFactory<Program>>
  {
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public GetById(CustomWebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
    {
      _client = factory.CreateClient();
      _outputHelper = outputHelper;
    }

    [Fact]
    public async Task GetsExistingAppointment()
    {
      // get schedule
      var listResult = await _client.GetAndDeserializeAsync<ListScheduleResponse>(ListScheduleRequest.Route, _outputHelper);
      var schedule = listResult.Schedules.First();
      _outputHelper.WriteLine($"Schedule: {schedule}");

      string listRoute = ListAppointmentRequest.Route.Replace("{ScheduleId}", schedule.Id.ToString());

      _outputHelper.WriteLine($"Route: {listRoute}");

      var listResponse = await _client.GetAndDeserializeAsync<ListAppointmentResponse>(listRoute, _outputHelper);

      var firstAppt = listResponse.Appointments.First();
      _outputHelper.WriteLine(firstAppt.ToString());

      string route = GetByIdAppointmentRequest.Route.Replace("{AppointmentId}", firstAppt.AppointmentId.ToString());
      route = route.Replace("{ScheduleId}", schedule.Id.ToString());
      var result = await _client.GetAndDeserializeAsync<GetByIdAppointmentResponse>(route, _outputHelper);

      Assert.Equal(firstAppt.PatientId, result.Appointment.PatientId);
      Assert.Equal(firstAppt.PatientName, result.Appointment.PatientName);
      Assert.Equal(firstAppt.RoomId, result.Appointment.RoomId);
      Assert.Equal(firstAppt.ToString(), result.Appointment.ToString());
    }
  }
}
