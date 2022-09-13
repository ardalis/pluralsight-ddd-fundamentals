using Ardalis.Specification.EntityFrameworkCore;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Infrastructure.Data
{
  // We are using the EfRepository from Ardalis.Specification
  // https://github.com/ardalis/Specification/blob/v5.1.0/ArdalisSpecificationEF/src/Ardalis.Specification.EF/RepositoryBaseOfT.cs
  public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
  {
    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
  }
}
