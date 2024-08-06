using FrontDesk.Core.Events;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class AppointmentScheduledHandler : AppointmentEventHandler<AppointmentScheduledEvent>
  {
    public AppointmentScheduledHandler(IHubContext<ScheduleHub> hubContext) : base(hubContext)
    {
    }

    protected override string Message => "was JUST SCHEDULED";
  }
}
