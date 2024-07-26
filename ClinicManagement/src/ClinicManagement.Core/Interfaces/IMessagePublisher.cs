using PluralsightDdd.SharedKernel;

namespace ClinicManagement.Core.Interfaces
{
  public interface IMessagePublisher
  {
    void Publish(BaseIntegrationEvent applicationEvent);
  }
}
