using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Infrastructure.Data
{
  public class CachedRepository<T> : IReadRepository<T> where T : class, IAggregateRoot
  {
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedRepository<T>> _logger;
    private readonly EfRepository<T> _sourceRepository;
    private MemoryCacheEntryOptions _cacheOptions;

    public CachedRepository(IMemoryCache cache,
        ILogger<CachedRepository<T>> logger,
        EfRepository<T> sourceRepository)
    {
      _cache = cache;
      _logger = logger;
      _sourceRepository = sourceRepository;

      _cacheOptions = new MemoryCacheEntryOptions()
          .SetAbsoluteExpiration(relative: TimeSpan.FromSeconds(10));
    }

    public Task<T> AddAsync(T entity)
    {
      return _sourceRepository.AddAsync(entity);
    }

    public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
      return _sourceRepository.CountAsync(specification, cancellationToken);
    }

    public Task DeleteAsync(T entity)
    {
      return _sourceRepository.DeleteAsync(entity);
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities)
    {
      return _sourceRepository.DeleteRangeAsync(entities);
    }

    public Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
      string key = $"{typeof(T).Name}-{id}";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreate(key, entry =>
      {
        entry.SetOptions(_cacheOptions);
        _logger.LogWarning("Fetching source data for " + key);
        return _sourceRepository.GetByIdAsync(id, cancellationToken);
      });
    }

    public Task<T> GetBySpecAsync<Spec>(Spec specification,
      CancellationToken cancellationToken = default) where Spec : ISingleResultSpecification, ISpecification<T>
    {
      if (specification.CacheEnabled)
      {
        string key = $"{specification.CacheKey}-GetBySpecAsync";
        _logger.LogInformation("Checking cache for " + key);
        return _cache.GetOrCreate(key, entry =>
        {
          entry.SetOptions(_cacheOptions);
          _logger.LogWarning("Fetching source data for " + key);
          return _sourceRepository.GetBySpecAsync(specification, cancellationToken);
        });
      }
      return _sourceRepository.GetBySpecAsync(specification);
    }

    public Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
      CancellationToken cancellationToken = default)
    {
      if (specification.CacheEnabled)
      {
        string key = $"{specification.CacheKey}-GetBySpecAsync";
        _logger.LogInformation("Checking cache for " + key);
        return _cache.GetOrCreate(key, entry =>
        {
          entry.SetOptions(_cacheOptions);
          _logger.LogWarning("Fetching source data for " + key);
          return _sourceRepository.GetBySpecAsync(specification, cancellationToken);
        });
      }
      return _sourceRepository.GetBySpecAsync(specification, cancellationToken);
    }

    public Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
      string key = $"{typeof(T).Name}-List";
      _logger.LogInformation($"Checking cache for {key}");
      return _cache.GetOrCreate(key, entry =>
      {
        entry.SetOptions(_cacheOptions);
        _logger.LogWarning($"Fetching source data for {key}");
        return _sourceRepository.ListAsync(cancellationToken);
      });
    }

    public Task<List<T>> ListAsync(ISpecification<T> specification,
      CancellationToken cancellationToken = default)
    {
      if (specification.CacheEnabled)
      {
        string key = $"{specification.CacheKey}-ListAsync";
        _logger.LogInformation($"Checking cache for {key}");
        return _cache.GetOrCreate(key, entry =>
        {
          entry.SetOptions(_cacheOptions);
          _logger.LogWarning($"Fetching source data for {key}");
          return _sourceRepository.ListAsync(specification, cancellationToken);
        });
      }
      return _sourceRepository.ListAsync(specification, cancellationToken);
    }

    public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification,
      CancellationToken cancellationToken = default)
    {
      if (specification.CacheEnabled)
      {
        string key = $"{specification.CacheKey}-ListAsync";
        _logger.LogInformation($"Checking cache for {key}");
        return _cache.GetOrCreate(key, entry =>
        {
          entry.SetOptions(_cacheOptions);
          _logger.LogWarning($"Fetching source data for {key}");
          return _sourceRepository.ListAsync(specification, cancellationToken);
        });
      }
      return _sourceRepository.ListAsync(specification, cancellationToken);
    }

    public Task SaveChangesAsync()
    {
      return _sourceRepository.SaveChangesAsync();
    }

    public Task UpdateAsync(T entity)
    {
      return _sourceRepository.UpdateAsync(entity);
    }
  }
}
