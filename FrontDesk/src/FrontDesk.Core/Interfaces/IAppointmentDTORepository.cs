using BlazorShared.Models.Appointment;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Interfaces
{
  public interface IAppointmentDTORepository
  {
    AppointmentDto GetFromAppointment(Appointment appointment);
  }
}
