using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FrontDesk.Api.Hubs
{
  public class ScheduleHub : Hub
  {
    public Task UpdateScheduleAsync(string message)
    {
      return Clients.All.SendAsync("ReceiveMessage", message);
    }
  }
}
