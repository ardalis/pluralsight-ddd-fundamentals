using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Blazor.Services
{
  public class AppointmentService
  {
    private readonly HttpService _httpService;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(HttpService httpService, ILogger<AppointmentService> logger)
    {
      _httpService = httpService;
      _logger = logger;
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentRequest appointment)
    {
      return (await _httpService.HttpPostAsync<CreateAppointmentResponse>("appointments", appointment)).Appointment;
    }

    public async Task<AppointmentDto> EditAsync(UpdateAppointmentRequest appointment)
    {
      return (await _httpService.HttpPutAsync<UpdateAppointmentResponse>("appointments", appointment)).Appointment;
    }

    public async Task DeleteAsync(Guid appointmentId)
    {
      await _httpService.HttpDeleteAsync<DeleteAppointmentResponse>("appointments", appointmentId);
    }

    public async Task<AppointmentDto> GetByIdAsync(Guid appointmentId)
    {
      return (await _httpService.HttpGetAsync<GetByIdAppointmentResponse>($"appointments/{appointmentId}")).Appointment;
    }

    public async Task<List<AppointmentDto>> ListPagedAsync(int pageSize)
    {
      _logger.LogInformation("Fetching appointments from API.");

      return (await _httpService.HttpGetAsync<ListAppointmentResponse>($"appointments")).Appointments;
    }

    public async Task<List<AppointmentDto>> ListAsync()
    {
      _logger.LogInformation("Fetching appointments from API.");

      return (await _httpService.HttpGetAsync<ListAppointmentResponse>($"appointments")).Appointments;
    }
  }
}
