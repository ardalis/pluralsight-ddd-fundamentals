using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Blazor.Services
{
  public class ConfigurationService
  {
    private readonly HttpService _httpService;
    private readonly ILogger<ConfigurationService> _logger;

    public ConfigurationService(HttpService httpService, ILogger<ConfigurationService> logger)
    {
      _httpService = httpService;
      _logger = logger;
    }

    public async Task<DateTime> ReadAsync()
    {
      _logger.LogInformation("Read today date/time from configuration.");

      return Convert.ToDateTime(await _httpService.HttpGetAsync($"configurations"));
    }
  }
}
