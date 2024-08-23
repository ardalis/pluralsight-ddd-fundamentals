using System.Threading.Tasks;
using FrontDesk.Contracts;
using MassTransit;
using MediatR;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic.MessageHandlers;

public class AppointmentScheduledConsumer : IConsumer<AppointmentScheduledIntegrationEvent>
{
  private readonly IMediator _mediator;

  public AppointmentScheduledConsumer(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task Consume(ConsumeContext<AppointmentScheduledIntegrationEvent> context)
  {
    var message = context.Message;
    
    var command = new SendAppointmentConfirmationCommand()
    {
      AppointmentId = message.AppointmentId,
      AppointmentType = message.AppointmentType,
      ClientEmailAddress = message.ClientEmailAddress,
      ClientName = message.ClientName,
      DoctorName = message.DoctorName,
      PatientName = message.PatientName,
      AppointmentStartDateTime = message.AppointmentStartDateTime.DateTime,
    };
    await _mediator.Send(command);
  }
}
