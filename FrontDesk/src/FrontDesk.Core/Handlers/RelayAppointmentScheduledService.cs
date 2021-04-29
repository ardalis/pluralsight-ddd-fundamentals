using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.Events;
using FrontDesk.Core.Events.IntegrationEvents;
using FrontDesk.Core.Exceptions;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.SyncedAggregates.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Handlers
{
  /// <summary>
  /// Post CreateConfirmationEmailMessage to message bus/queue to allow confirmation emails to be sent
  /// </summary>
  public class RelayAppointmentScheduledHandler : INotificationHandler<AppointmentScheduledEvent>
  {
    private readonly IReadRepository<Doctor> _doctorRepository;
    private readonly IReadRepository<Client> _clientRepository;
    private readonly IReadRepository<AppointmentType> _appointmentTypeRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<RelayAppointmentScheduledHandler> _logger;

    public RelayAppointmentScheduledHandler(
      IReadRepository<Doctor> doctorRepository,
      IReadRepository<Client> clientRepository,
      IReadRepository<AppointmentType> appointmentTypeRepository,
      IMessagePublisher messagePublisher,
      ILogger<RelayAppointmentScheduledHandler> logger)
    {
      _doctorRepository = doctorRepository;
      _clientRepository = clientRepository;
      _appointmentTypeRepository = appointmentTypeRepository;
      _messagePublisher = messagePublisher;
      _logger = logger;
    }

    public async Task Handle(AppointmentScheduledEvent appointmentScheduledEvent,
      CancellationToken cancellationToken)
    {
      _logger.LogInformation("Handling appointmentScheduledEvent");
      // we are translating from a domain event to an integration event here
      var newMessage = new AppointmentScheduledIntegrationEvent();

      var appt = appointmentScheduledEvent.AppointmentScheduled;

      // if this is slow these can be parallelized or cached. MEASURE before optimizing.
      var doctor = await _doctorRepository.GetByIdAsync(appt.DoctorId);
      if (doctor == null) throw new DoctorNotFoundException(appt.DoctorId);

      var clientWithPatientsSpec = new ClientByIdIncludePatientsSpecification(appt.ClientId);
      var client = await _clientRepository.GetBySpecAsync(clientWithPatientsSpec);
      if (client == null) throw new ClientNotFoundException(appt.ClientId);

      var patient = client.Patients.FirstOrDefault(p => p.Id == appt.PatientId);
      if (patient == null) throw new PatientNotFoundException(appt.PatientId);

      var apptType = await _appointmentTypeRepository.GetByIdAsync(appt.AppointmentTypeId);
      if (apptType == null) throw new AppointmentTypeNotFoundException(appt.AppointmentTypeId);

      newMessage.AppointmentId = appt.Id;
      newMessage.AppointmentStartDateTime = appointmentScheduledEvent.AppointmentScheduled.TimeRange.Start;
      newMessage.ClientName = client.FullName;
      newMessage.ClientEmailAddress = client.EmailAddress;
      newMessage.DoctorName = doctor.Name;
      newMessage.PatientName = patient.Name;
      newMessage.AppointmentType = apptType.Name;

      _messagePublisher.Publish(newMessage);
      _logger.LogInformation($"Message published. {newMessage.PatientName}");
    }
  }
}
