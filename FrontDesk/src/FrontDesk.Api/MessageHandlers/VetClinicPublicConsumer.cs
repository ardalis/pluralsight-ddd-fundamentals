using System.Threading.Tasks;
using FrontDesk.Api.Hubs;
using FrontDesk.Core.Handlers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using VetClinicPublic.Contracts;

namespace FrontDesk.Api.MessageHandlers;

public class VetClinicPublicConsumer : IConsumer<AppointmentConfirmLinkClickedIntegrationEvent>
{
  private readonly ILogger<ClinicManagementDoctorConsumer> _logger;
  private readonly IMediator _mediator;
  private readonly IHubContext<ScheduleHub> _scheduleHub;

  public VetClinicPublicConsumer(ILogger<ClinicManagementDoctorConsumer> logger, IMediator mediator,
    IHubContext<ScheduleHub> scheduleHub)
  {
    _logger = logger;
    _mediator = mediator;
    _scheduleHub = scheduleHub;
  }

  public async Task Consume(ConsumeContext<AppointmentConfirmLinkClickedIntegrationEvent> context)
  {
    var message = context.Message;
    _logger.LogInformation(" [x] Received {Message}", message);

    var appEvent = new ConfirmAppointmentCommand(message.DateTimeEventOccurred)
    {
      AppointmentId = message.AppointmentId
    };
    await _mediator.Send(appEvent);
  }
}
