using System;

namespace BlazorShared.Models.Appointment
{
  public class CreateAppointmentRequest : BaseRequest
  {
    public int PatientId { get; set; }
    public Guid ScheduleId { get; set; }
    public int AppointmentTypeId { get; set; }
    public int ClientId { get; set; }
    public int RoomId { get; set; }
    public DateTime DateOfAppointment { get; set; }
    public int SelectedDoctor { get; set; }
    public string Details { get; set; }
  }
}
