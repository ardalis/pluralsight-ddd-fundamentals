using System.Threading.Tasks;
using FrontDesk.Contracts;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic.MessageHandlers;

public class AppointmentScheduledConsumer : IConsumer<AppointmentScheduledIntegrationEvent>
{
  private readonly ILogger<AppointmentScheduledConsumer> _logger;
  private readonly IMediator _mediator;
  public AppointmentScheduledConsumer(ILogger<AppointmentScheduledConsumer> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Consume(ConsumeContext<AppointmentScheduledIntegrationEvent> context)
  {
    var message = context.Message;
    
    _logger.LogInformation("Handling Message: {Message}", message);
    
    var command = new SendAppointmentConfirmationCommand()
    {
      AppointmentId = message.AppointmentId,
      AppointmentType = message.AppointmentType,
      ClientEmailAddress = message.ClientEmailAddress,
      ClientName = message.ClientName,
      DoctorName = message.DoctorName,
      PatientName = message.PatientName,
      AppointmentStartDateTime = message.AppointmentStartDateTime.DateTime,
    };
    await _mediator.Send(command);
  }
}
