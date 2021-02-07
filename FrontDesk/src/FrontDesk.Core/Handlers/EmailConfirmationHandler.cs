using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events.ApplicationEvents;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.Specifications;
using MediatR;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Services
{
  public class EmailConfirmationHandler : INotificationHandler<AppointmentConfirmedEvent>
  {
    private readonly IRepository _scheduleRepository;

    private readonly IApplicationSettings _settings;

    public EmailConfirmationHandler(IRepository scheduleRepository, IApplicationSettings settings)
    {
      _scheduleRepository = scheduleRepository;
      _settings = settings;
    }

    public async Task Handle(AppointmentConfirmedEvent appointmentConfirmedEvent, CancellationToken cancellationToken)
    {
      var scheduleSpec = new ScheduleForDateAndClinicSpecification(_settings.ClinicId, _settings.TestDate);
      // Note: In this demo this only works for appointments scheduled on TestDate
      var schedule = (await _scheduleRepository.ListAsync<Schedule, Guid>(scheduleSpec)).FirstOrDefault();
      Guard.Against.Null(schedule, nameof(Schedule));

      var appointmentToConfirm = schedule.Appointments.FirstOrDefault(a => a.Id == appointmentConfirmedEvent.AppointmentId);

      appointmentToConfirm.Confirm(appointmentConfirmedEvent.DateTimeEventOccurred);

      await _scheduleRepository.UpdateAsync<Schedule, Guid>(schedule);
    }
  }
}
