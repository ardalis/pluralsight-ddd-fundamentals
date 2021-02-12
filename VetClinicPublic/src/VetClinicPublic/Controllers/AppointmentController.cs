using System;
using Microsoft.AspNetCore.Mvc;

namespace VetClinicPublic.Web.Controllers
{
  public class AppointmentController : Controller
  {
    public ActionResult Confirm(Guid id)
    {
      // TODO: Send message to Queue confirming appointment
      //var messagingConfig = new MessagingConfig();
      //messagingConfig.SendConfirmationMessageToScheduler(new AppointmentConfirmedEvent(id));
      return View();
    }
  }
}
