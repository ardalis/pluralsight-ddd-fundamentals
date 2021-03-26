using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models.Schedule;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Blazor.Services
{
  public class ScheduleService
  {
    private readonly HttpService _httpService;
    private readonly ILogger<ScheduleService> _logger;

    public ScheduleService(HttpService httpService,
      ILogger<ScheduleService> logger)
    {
      _httpService = httpService;
      _logger = logger;
    }

    public async Task<List<ScheduleDto>> ListAsync()
    {
      _logger.LogInformation("Fetching schedules from API.");

      var route = ListScheduleRequest.Route;
      return (await _httpService.HttpGetAsync<ListScheduleResponse>(route)).Schedules;
    }
  }
}
