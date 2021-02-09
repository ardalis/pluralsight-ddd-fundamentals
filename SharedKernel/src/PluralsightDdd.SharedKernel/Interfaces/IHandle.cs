using System.Threading.Tasks;

namespace PluralsightDdd.SharedKernel.Interfaces
{
  public interface IHandle<T> where T : BaseDomainEvent
  {
    Task HandleAsync(T args);
  }
}
