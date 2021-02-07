using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Interfaces
{
  public interface IAppointmentDTORepository
  {
    AppointmentDto GetFromAppointment(Appointment appointment);
  }
}
