using System.Threading.Tasks;

namespace FrontDesk.Core.Interfaces
{
  public interface IMessagePublisher
  {
    Task Publish(object eventToPublish);
  }
}
