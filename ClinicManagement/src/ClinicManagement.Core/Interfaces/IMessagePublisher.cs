using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Core.Interfaces
{
  public interface IMessagePublisher
  {
    void Publish(IApplicationEvent applicationEvent);
  }
}