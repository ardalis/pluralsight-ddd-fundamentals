using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using VetClinicPublic.Web.Interfaces;

namespace VetClinicPublic.Web.Controllers
{
  public class HomeController : Controller
  {
    //private readonly IProducingService _producingService;
    private readonly ILogger<HomeController> _logger;
    private readonly ISendEmail _emailSender;
    private readonly SiteConfiguration _siteConfig;

    public HomeController(
            ILogger<HomeController> logger,
            ISendEmail emailSender,
            IOptions<SiteConfiguration> siteConfigOptions
            )
    {
      //_producingService = producingService;
      _logger = logger;
      _emailSender = emailSender;
      _siteConfig = siteConfigOptions.Value;
    }

    public ActionResult Index()
    {
      ViewBag.PapercutManagementPort = _siteConfig.PapercutManagementPort;
      return View();
    }

    public ActionResult TestEmail()
    {
      // sends a test email to confirm SMTP configuration is working
      _emailSender.SendEmail("test@test.com", "donotreply@test.com",
        "Test Email from /TestEmail path",
        "This is just a test.");

      return Ok("Email sent");
    }
  }
}
