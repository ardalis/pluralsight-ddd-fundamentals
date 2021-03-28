using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class ScheduleHub : Hub
  {
    public Task UpdateScheduleAsync(string message)
    {
      // TODO: Avoid having messages appear to the user initiating them
      return Clients.Others.SendAsync("ReceiveMessage", message);
    }
  }
}
