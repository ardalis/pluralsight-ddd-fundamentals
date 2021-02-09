using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace PluralsightDdd.SharedKernel.Interfaces
{
  public interface IRepository
  {
    Task<T> GetByIdAsync<T, TId>(TId id) where T : BaseEntity<TId>, IAggregateRoot;
    Task<T> GetAsync<T, TId>(ISpecification<T> spec) where T : BaseEntity<TId>, IAggregateRoot;

    Task<List<T>> ListAsync<T, TId>() where T : BaseEntity<TId>, IAggregateRoot;
    Task<List<T>> ListAsync<T, TId>(ISpecification<T> spec) where T : BaseEntity<TId>, IAggregateRoot;

    Task<int> CountAsync<T, TId>(ISpecification<T> spec) where T : BaseEntity<TId>, IAggregateRoot;

    Task<T> AddAsync<T, TId>(T entity) where T : BaseEntity<TId>, IAggregateRoot;
    Task UpdateAsync<T, TId>(T entity) where T : BaseEntity<TId>, IAggregateRoot;
    Task DeleteAsync<T, TId>(T entity) where T : BaseEntity<TId>, IAggregateRoot;
  }
}
