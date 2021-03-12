using System;
using Microsoft.AspNetCore.Mvc;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic.Web.Controllers
{
  public class AppointmentController : Controller
  {
    private readonly IMessagePublisher _messagePublisher;

    public AppointmentController(IMessagePublisher messagePublisher)
    {
      _messagePublisher = messagePublisher;
    }

    [HttpGet("appointment/confirm/{id}")]
    public ActionResult Confirm(Guid id)
    {
      var appEvent = new AppointmentConfirmedEvent(id);
      _messagePublisher.Publish(appEvent);
      // 4A071770-35A3-42D7-A1AE-4244870BA158
      return View();
    }
  }
}
