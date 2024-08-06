using System.Threading.Tasks;
using System.Threading;
using FrontDesk.Core.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public abstract class AppointmentEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : AppointmentEvent
  {
    private readonly IHubContext<ScheduleHub> _hubContext;
    protected abstract string Message { get; }

    protected AppointmentEventHandler(IHubContext<ScheduleHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
      return _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{notification.Appointment.Title} {Message}", cancellationToken);
    }
  }
}
