using System.Threading.Tasks;
using FrontDesk.Core.Events.IntegrationEvents;
namespace FrontDesk.Core.Interfaces
{
  public interface IMessagePublisher
  {
    Task Publish(object eventToPublish);
  }
}
