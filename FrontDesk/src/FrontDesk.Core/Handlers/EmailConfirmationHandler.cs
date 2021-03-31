using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events.ApplicationEvents;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Handlers
{
  /// <summary>
  /// This handler responds to incoming messages saying a user has confirmed an appointment
  /// </summary>
  public class EmailConfirmationHandler : INotificationHandler<AppointmentConfirmedAppEvent>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IReadRepository<Schedule> _scheduleReadRepository;
    private readonly IApplicationSettings _settings;
    private readonly ILogger<EmailConfirmationHandler> _logger;

    public EmailConfirmationHandler(IRepository<Schedule> scheduleRepository,
      IReadRepository<Schedule> scheduleReadRepository,
      IApplicationSettings settings,
      ILogger<EmailConfirmationHandler> logger)
    {
      _scheduleRepository = scheduleRepository;
      _scheduleReadRepository = scheduleReadRepository;
      _settings = settings;
      _logger = logger;
    }

    public async Task Handle(AppointmentConfirmedAppEvent appointmentConfirmedEvent,
      CancellationToken cancellationToken)
    {
      _logger.LogInformation($"Handling appointment confirmation: {appointmentConfirmedEvent.AppointmentId}");

      var scheduleSpec = new ScheduleForClinicAndDateWithAppointmentsSpec(_settings.ClinicId, _settings.TestDate);
      // Note: In this demo this only works for appointments scheduled on TestDate
      var schedule = (await _scheduleReadRepository.GetBySpecAsync(scheduleSpec));
      Guard.Against.Null(schedule, nameof(Schedule));

      var appointmentToConfirm = schedule.Appointments.FirstOrDefault(a => a.Id == appointmentConfirmedEvent.AppointmentId);

      appointmentToConfirm.Confirm(appointmentConfirmedEvent.DateOccurred);

      await _scheduleRepository.UpdateAsync(schedule);
    }
  }
}
