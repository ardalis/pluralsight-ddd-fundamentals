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
public class ClinicManagementClientConsumer : IConsumer<ClientUpdatedIntegrationEvent>
{
  private readonly ILogger<ClinicManagementClientConsumer> _logger;
  private readonly IMediator _mediator;
  private readonly IHubContext<ScheduleHub> _scheduleHub;

  public ClinicManagementClientConsumer(ILogger<ClinicManagementClientConsumer> logger, IMediator mediator,
    IHubContext<ScheduleHub> scheduleHub)
  {
    _logger = logger;
    _mediator = mediator;
    _scheduleHub = scheduleHub;
  }

  public async Task Consume(ConsumeContext<ClientUpdatedIntegrationEvent> context)
  {
    var message = context.Message;
    _logger.LogInformation(" [x] Received {Message}", message);

    await HandleAsync(message);
  }

  private async Task HandleAsync(ClientUpdatedIntegrationEvent message)
  {
    var command = new UpdateClientCommand
    {
      Id = message.Id,
      Name = message.Name
    };
    await _mediator.Send(command);

    // TODO: Only send notification if changes occurred
    string notification = $"Client {message.Name} updated in Clinic Management.";
    await _scheduleHub.Clients.All.SendAsync("ReceiveMessage", notification);
  }
}
