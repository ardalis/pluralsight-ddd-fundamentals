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
    private readonly IRepository _repository;
    private readonly IMessagePublisher _messagePublisher;

    public RelayAppointmentScheduledService(IRepository repository,
        IMessagePublisher messagePublisher)
    {
      _repository = repository;
      _messagePublisher = messagePublisher;
    }

    public async Task Handle(AppointmentScheduledEvent appointmentScheduledEvent, CancellationToken cancellationToken)
    {
      // we are translating from a domain event to an application event here
      var newMessage = new CreateConfirmationEmailMessage();

      var appt = appointmentScheduledEvent.AppointmentScheduled;

      // if this is slow these can be parallelized or cached. MEASURE before optimizing.
      var doctor = await _repository.GetByIdAsync<Doctor, int>(appt.DoctorId.Value);
      var client = await _repository.GetByIdAsync<Client, int>(appt.ClientId);
      var patient = await _repository.GetByIdAsync<Patient, int>(appt.PatientId);
      var apptType = await _repository.GetByIdAsync<AppointmentType, int>(appt.AppointmentTypeId);

      newMessage.AppointmentDateTime = appointmentScheduledEvent.AppointmentScheduled.TimeRange.Start;
      newMessage.ClientName = client.FullName;
      newMessage.ClientEmailAddress = client.EmailAddress;
      newMessage.DoctorName = doctor.Name;
      newMessage.PatientName = patient.Name;
      newMessage.ProcedureName = apptType.Name;

      _messagePublisher.Publish(newMessage);
    }
  }
}
