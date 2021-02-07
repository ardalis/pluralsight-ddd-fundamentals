using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class AppointmentScheduledHandler : INotificationHandler<AppointmentScheduledEvent>
  {
    private readonly IHubContext<ScheduleHub> _hubContext;

    public AppointmentScheduledHandler(IHubContext<ScheduleHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public Task Handle(AppointmentScheduledEvent notification, CancellationToken cancellationToken)
    {
      return _hubContext.Clients.All.SendAsync("ReceiveMessage", notification.AppointmentScheduled.Title + " was JUST SCHEDULED", cancellationToken: cancellationToken);
    }
  }
}
