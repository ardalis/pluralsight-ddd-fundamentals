using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Infrastructure.Data
{
  public class EfRepository : IRepository
  {
    private readonly AppDbContext _dbContext;

    public EfRepository(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public T GetById<T, TId>(TId id) where T : BaseEntity<TId>, IAggregateRoot
    {
      return _dbContext.Set<T>().SingleOrDefault(e => e.Id.Equals(id));
    }

    public Task<T> GetByIdAsync<T, TId>(TId id) where T : BaseEntity<TId>, IAggregateRoot
    {
      return _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id.Equals(id));
    }

    public Task<List<T>> ListAsync<T, TId>() where T : BaseEntity<TId>, IAggregateRoot
    {
      return _dbContext.Set<T>().ToListAsync();
    }

    public Task<List<T>> ListAsync<T, TId>(ISpecification<T> spec) where T : BaseEntity<TId>, IAggregateRoot
    {
      var specificationResult = ApplySpecification<T, TId>(spec);
      return specificationResult.ToListAsync();
    }

    public async Task<T> AddAsync<T, TId>(T entity) where T : BaseEntity<TId>, IAggregateRoot
    {
      await _dbContext.Set<T>().AddAsync(entity);
      await _dbContext.SaveChangesAsync();

      return entity;
    }

    public Task UpdateAsync<T, TId>(T entity) where T : BaseEntity<TId>, IAggregateRoot
    {
      _dbContext.Set<T>().Update(entity);
      return _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync<T, TId>(T entity) where T : BaseEntity<TId>, IAggregateRoot
    {
      _dbContext.Set<T>().Remove(entity);
      return _dbContext.SaveChangesAsync();
    }

    public Task<int> CountAsync<T, TId>(ISpecification<T> spec) where T : BaseEntity<TId>, IAggregateRoot
    {
      var specificationResult = ApplySpecification<T, TId>(spec);
      return specificationResult.CountAsync();
    }

    private IQueryable<T> ApplySpecification<T, TId>(ISpecification<T> spec) where T : BaseEntity<TId>
    {
      var evaluator = new SpecificationEvaluator<T>();
      return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
    }
  }
}
