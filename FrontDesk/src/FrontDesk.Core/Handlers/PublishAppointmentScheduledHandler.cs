using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.Events;
using FrontDesk.Core.Events.ApplicationEvents;
using FrontDesk.Core.Interfaces;
using MediatR;

namespace FrontDesk.Core.Handlers
{
  public class PublishAppointmentScheduledHandler : INotificationHandler<AppointmentScheduledEvent>
  {
    private readonly IMessagePublisher _messagePublisher;

    public PublishAppointmentScheduledHandler(IMessagePublisher messagePublisher)
    {
      _messagePublisher = messagePublisher;
    }
    public Task Handle(AppointmentScheduledEvent notification, CancellationToken cancellationToken)
    {
      var appt = notification.AppointmentScheduled;
      var appEvent = new AppointmentScheduledAppEvent()
      {
        AppointmentScheduled = new AppointmentScheduledAppEvent.AppointmentScheduledDTO
        {
          AppointmentId = appt.Id,
          AppointmentType = appt.AppointmentTypeId.ToString(),
          ClientEmailAddress = appt.ClientId.ToString(),
          ClientName = appt.ClientId.ToString(),
          DoctorName = appt.DoctorId.ToString(),
          End = appt.TimeRange.End.ToLocalTime(),
          PatientName = appt.PatientId.ToString(),
          Start = appt.TimeRange.Start.ToLocalTime()
        }
      };

      // send it
      _messagePublisher.Publish(appEvent);

      return Task.CompletedTask;
    }
  }
}
