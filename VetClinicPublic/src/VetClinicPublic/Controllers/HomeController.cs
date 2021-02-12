using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace VetClinicPublic.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly IProducingService _producingService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IProducingService producingService,
            ILogger<HomeController> logger)
    {
      _producingService = producingService;
      _logger = logger;
    }

    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/send")]
    public async Task<ActionResult> Send()
    {
      _logger.LogInformation($"Sending messages with {typeof(IProducingService)}.");
      var message = new { message = "text" };
      await _producingService.SendAsync(message, "consumption.exchange", "routing.key");
      return Ok(message);
    }
  }
}
