using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.SyncedAggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Infrastructure.Data
{
  public class AppDbContext : DbContext
  {
    private readonly IMediator _mediator;

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator)
        : base(options)
    {
      _mediator = mediator;
    }

    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentType> AppointmentTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    // TODO: Use DbContext.SavedChanges event and handler to support events
    // https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/events
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

      // ignore events if no dispatcher provided
      if (_mediator == null) return result;

      var entitiesWithEvents = ChangeTracker
          .Entries()
          .Select(e => e.Entity as BaseEntity<Guid>)
          .Where(e => e?.Events != null && e.Events.Any())
          .ToArray();

      foreach (var entity in entitiesWithEvents)
      {
        var events = entity.Events.ToArray();
        entity.Events.Clear();
        foreach (var domainEvent in events)
        {
          await _mediator.Publish(domainEvent).ConfigureAwait(false);
        }
      }

      return result;
    }

    public override int SaveChanges()
    {
      return SaveChangesAsync().GetAwaiter().GetResult();
    }
  }
}
