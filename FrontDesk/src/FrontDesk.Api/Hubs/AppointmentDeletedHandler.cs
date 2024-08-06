using FrontDesk.Core.Events;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class AppointmentDeletedHandler : AppointmentEventHandler<AppointmentDeletedEvent>
  {
    public AppointmentDeletedHandler(IHubContext<ScheduleHub> hubContext) : base(hubContext)
    {
    }

    protected override string Message => "was deleted";
  }
}
