using System;
using MediatR;

namespace FrontDesk.Core.Handlers
{
  // This is fired by the message queue handler when an appointment should
  // be marked confirmed. It happens before the appointment is confirmed in
  // the model.
  public class ConfirmAppointmentCommand : IRequest
  {
    public ConfirmAppointmentCommand() : this(DateTimeOffset.Now)
    {
    }

    public ConfirmAppointmentCommand(DateTimeOffset dateOccurred)
    {
      DateOccurred = dateOccurred;
    }

    public DateTimeOffset DateOccurred { get; set; }

    public Guid AppointmentId { get; set; }
  }
}
