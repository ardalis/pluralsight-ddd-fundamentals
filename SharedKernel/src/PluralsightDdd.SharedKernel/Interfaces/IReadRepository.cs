using Ardalis.Specification;

namespace PluralsightDdd.SharedKernel.Interfaces
{
  public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
  {
  }
}
