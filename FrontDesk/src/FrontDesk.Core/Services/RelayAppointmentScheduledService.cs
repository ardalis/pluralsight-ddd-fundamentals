using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using FrontDesk.Core.Exceptions;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.Specifications;
using MediatR;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Services
{
  /// <summary>
  /// Post CreateConfirmationEmailMessage to message bus/queue to allow confirmation emails to be sent
  /// </summary>
  public class RelayAppointmentScheduledService : INotificationHandler<AppointmentScheduledEvent>
  {
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IRepository<Client> _clientRepository;
    private readonly IRepository<AppointmentType> _appointmentTypeRepository;
    private readonly IMessagePublisher _messagePublisher;

    public RelayAppointmentScheduledService(IRepository<Doctor> doctorRepository,
      IRepository<Client> clientRepository,
      IRepository<AppointmentType> appointmentTypeRepository,
      IMessagePublisher messagePublisher)
    {
      _doctorRepository = doctorRepository;
      _clientRepository = clientRepository;
      _appointmentTypeRepository = appointmentTypeRepository;
      _messagePublisher = messagePublisher;
    }

    public async Task Handle(AppointmentScheduledEvent appointmentScheduledEvent, CancellationToken cancellationToken)
    {
      // we are translating from a domain event to an application event here
      var newMessage = new CreateConfirmationEmailMessage();

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

      newMessage.AppointmentDateTime = appointmentScheduledEvent.AppointmentScheduled.TimeRange.Start;
      newMessage.ClientName = client.FullName;
      newMessage.ClientEmailAddress = client.EmailAddress;
      newMessage.DoctorName = doctor.Name;
      newMessage.PatientName = patient.Name;
      newMessage.ProcedureName = apptType.Name;

      // TODO: uncomment this after finish ServiceBrokerMessagePublisher 
      // messagePublisher.Publish(newMessage);
    }
  }
}
