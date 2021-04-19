using System.Linq;
using Ardalis.Specification.EntityFrameworkCore;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Infrastructure.Data.Migrations;
using PluralsightDdd.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace FrontDesk.Infrastructure.Data
{
  // We are using the EfRepository from Ardalis.Specification
  // https://github.com/ardalis/Specification/blob/v5/ArdalisSpecificationEF/src/Ardalis.Specification.EF/RepositoryBaseOfT.cs
  public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
  {
    private readonly AppDbContext _dbContext;

    public EfRepository(AppDbContext dbContext) : base(dbContext)
    {
      _dbContext = dbContext;
    }

    public Schedule Get(int clinicId, DateTime date)
    {
      var endDate = date.AddDays(1);

      var schedule = _dbContext.Set<Schedule>()
          .Include(s => s.Appointments.Where(a => 
            a.TimeRange.Start > date && 
            a.TimeRange.End < endDate))

          .FirstOrDefault(schedule =>
            schedule.ClinicId == clinicId);

      return schedule;
    }
  }
}
