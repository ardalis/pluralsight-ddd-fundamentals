using System;
using System.Collections.Generic;
using BlazorShared.Models.Appointment;

namespace FrontDesk.Blazor.Shared.SchedulerComponent
{
  public class SchedulerService
  {
    public event Action RefreshRequested;
    public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

    public void RefreshAppointments(List<AppointmentDto> appointments)
    {
      Appointments = appointments;
      RefreshRequested?.Invoke();
    }
  }
}
