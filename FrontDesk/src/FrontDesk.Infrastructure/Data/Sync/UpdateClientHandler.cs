using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.SyncedAggregates;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Infrastructure.Data.Sync
{
  public class UpdateClientHandler : IRequestHandler<UpdateClientCommand>
  {
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UpdateClientHandler> _logger;

    public UpdateClientHandler(AppDbContext dbContext,
      ILogger<UpdateClientHandler> logger)
    {
      _dbContext = dbContext;
      _logger = logger;
    }

    public async Task<Unit> Handle(UpdateClientCommand request,
      CancellationToken cancellationToken)
    {
      _logger.LogInformation($"UpdateClientHandler updating Client {request.Name} to sync with Clinic Management.");
      var client = _dbContext.Set<Client>().Find(request.Id);
      var currentName = client.FullName;

      if (request.Name == currentName)
      {
        // no change
        return Unit.Value;
      }

      // use reflection to set name
      var type = client.GetType();
      var prop = type.GetProperty(nameof(client.FullName));
      prop.SetValue(client, request.Name, null);

      UpdateAppointmentTitles(request, currentName);

      _ = await _dbContext.SaveChangesAsync();

      return Unit.Value;
    }

    private void UpdateAppointmentTitles(UpdateClientCommand request, string currentName)
    {
      _logger.LogInformation($"UpdateClientHandler updating Client {request.Name}-{request.Id}'s appointments.");
      // update appointment titles that include client name
      var appointments = _dbContext.Set<Appointment>()
        .Where(appointment => appointment.ClientId == request.Id && appointment.Title.Contains(currentName))
        .ToList();
      _logger.LogInformation($"UpdateClientHandler found {appointments.Count} appointments to update.");

      foreach (var appointment in appointments)
      {
        // update with reflection
        var apptType = appointment.GetType();
        var apptTitle = apptType.GetProperty(nameof(appointment.Title));
        string newTitle = appointment.Title.Replace(currentName, request.Name);
        _logger.LogInformation($"Updating appointment {appointment.Title} to {newTitle}.");

        apptTitle.SetValue(appointment, newTitle, null);
      }
    }
  }
}
