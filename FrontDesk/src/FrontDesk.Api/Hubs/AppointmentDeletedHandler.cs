using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class AppointmentDeletedHandler : INotificationHandler<AppointmentDeletedEvent>
  {
    private readonly IHubContext<ScheduleHub> _hubContext;

    public AppointmentDeletedHandler(IHubContext<ScheduleHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public Task Handle(AppointmentDeletedEvent notification, CancellationToken cancellationToken)
    {
      return _hubContext.Clients.All.SendAsync("ReceiveMessage", notification.AppointmentDeleted.Title + " was deleted", 
        cancellationToken: cancellationToken);
    }
  }
}
