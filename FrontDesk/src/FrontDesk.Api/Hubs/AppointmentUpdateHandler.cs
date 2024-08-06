using FrontDesk.Core.Events;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class AppointmentUpdateHandler : AppointmentEventHandler<AppointmentUpdatedEvent>
  {
    public AppointmentUpdateHandler(IHubContext<ScheduleHub> hubContext) : base(hubContext)
    {
    }

    protected override string Message => "was updated";
  }
}
