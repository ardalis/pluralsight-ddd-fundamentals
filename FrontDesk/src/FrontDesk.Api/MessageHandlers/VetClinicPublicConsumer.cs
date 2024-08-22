using System.Threading.Tasks;
using FrontDesk.Api.Hubs;
using FrontDesk.Core.Events.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using VetClinicPublic.Contracts;

namespace FrontDesk.Api.MessageHandlers;

public class VetClinicPublicConsumer : IConsumer<AppointmentConfirmLinkClickedEvent>
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

  public async Task Consume(ConsumeContext<AppointmentConfirmLinkClickedEvent> context)
  {
    var message = context.Message;
    _logger.LogInformation(" [x] Received {@Message}", message);

    var appEvent = new AppointmentConfirmLinkClickedIntegrationEvent(message.DateTimeEventOccurred)
    {
      AppointmentId = message.AppointmentId
    };
    await _mediator.Publish(appEvent);
  }
}
