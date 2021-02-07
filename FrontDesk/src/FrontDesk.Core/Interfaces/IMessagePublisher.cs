using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Interfaces
{
  public interface IMessagePublisher
  {
    void Publish(IApplicationEvent applicationEvent);
  }
}