using System.Threading.Tasks;
using ClinicManagement.Contracts;
using FrontDesk.Api.Hubs;
using FrontDesk.Infrastructure.Data.Sync;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Api.MessageHandlers;

/// <summary>
/// TODO: Implement other kinds of updates
/// </summary>
public class ClinicManagementDoctorConsumer : IConsumer<DoctorCreatedIntegrationEvent>
{
  private readonly ILogger<ClinicManagementDoctorConsumer> _logger;
  private readonly IMediator _mediator;
  private readonly IHubContext<ScheduleHub> _scheduleHub;

  public ClinicManagementDoctorConsumer(ILogger<ClinicManagementDoctorConsumer> logger, IMediator mediator,
    IHubContext<ScheduleHub> scheduleHub)
  {
    _logger = logger;
    _mediator = mediator;
    _scheduleHub = scheduleHub;
  }

  public async Task Consume(ConsumeContext<DoctorCreatedIntegrationEvent> context)
  {
    var message = context.Message;
    _logger.LogInformation(" [x] Received {Message}", message);

    await HandleAsync(message);
  }

  private async Task HandleAsync(DoctorCreatedIntegrationEvent message)
  {
    var command = new CreateDoctorCommand { Id = message.Id, Name = message.Name };
    await _mediator.Send(command);

    string notification = $"New Doctor {message.Name} added in Clinic Management. ";
    await _scheduleHub.Clients.All.SendAsync("ReceiveMessage", notification);
  }
}
