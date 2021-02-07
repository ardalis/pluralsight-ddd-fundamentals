using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using FrontDesk.Core.Interfaces;
using MediatR;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Services
{
  /// <summary>
  /// Post CreateConfirmationEmailMessage to message bus/queue to allow confirmation emails to be sent
  /// </summary>
  public class RelayAppointmentScheduledService : INotificationHandler<AppointmentScheduledEvent>
  {
    private readonly IRepository _apptRepository;
    private readonly IMessagePublisher _messagePublisher;

    public RelayAppointmentScheduledService(IRepository apptRepository,
        IMessagePublisher messagePublisher)
    {
      this._apptRepository = apptRepository;
      _messagePublisher = messagePublisher;
    }

    public async Task Handle(AppointmentScheduledEvent appointmentScheduledEvent, CancellationToken cancellationToken)
    {
      // we are translating from a domain event to an application event here
      var newMessage = new CreateConfirmationEmailMessage();

      var doctor = await _apptRepository.GetByIdAsync<Doctor, int>(appointmentScheduledEvent.AppointmentScheduled.DoctorId.Value);

      newMessage.AppointmentDateTime = appointmentScheduledEvent.AppointmentScheduled.TimeRange.Start;
      newMessage.ClientName = appointmentScheduledEvent.AppointmentScheduled.Client.FullName;
      newMessage.ClientEmailAddress = appointmentScheduledEvent.AppointmentScheduled.Client.EmailAddress;
      newMessage.DoctorName = doctor.Name;
      newMessage.PatientName = appointmentScheduledEvent.AppointmentScheduled.Patient.Name;
      newMessage.ProcedureName = appointmentScheduledEvent.AppointmentScheduled.AppointmentType.Name;

      _messagePublisher.Publish(newMessage);
    }
  }
}
