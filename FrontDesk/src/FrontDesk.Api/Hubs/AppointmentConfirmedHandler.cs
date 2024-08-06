using FrontDesk.Core.Events;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class AppointmentConfirmedHandler : AppointmentEventHandler<AppointmentConfirmedEvent>
  {
    public AppointmentConfirmedHandler(IHubContext<ScheduleHub> hubContext) : base(hubContext)
    {
    }

    protected override string Message => "was confirmed";
  }
}
