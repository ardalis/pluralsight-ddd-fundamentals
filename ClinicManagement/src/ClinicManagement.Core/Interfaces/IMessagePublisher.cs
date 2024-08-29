using System.Threading.Tasks;
using PluralsightDdd.SharedKernel;

namespace ClinicManagement.Core.Interfaces
{
  public interface IMessagePublisher
  {
    Task Publish(object applicationEvent);
  }
}
