using System;

namespace BlazorShared.Models.Appointment
{
  public class UpdateAppointmentRequest : BaseRequest
  {
    public const string Route = "api/appointments";

    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public string Title { get; set; }
    public int RoomId { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentTypeId { get; set; }
    public DateTimeOffset Start { get; set; }

    public static UpdateAppointmentRequest FromDto(AppointmentDto appointmentDto)
    {
      return new UpdateAppointmentRequest()
      {
        Id = appointmentDto.AppointmentId,
        DoctorId = appointmentDto.DoctorId,
        Title = appointmentDto.Title,
        ScheduleId = appointmentDto.ScheduleId,
        RoomId = appointmentDto.RoomId,
        AppointmentTypeId = appointmentDto.AppointmentTypeId,
        Start = appointmentDto.Start
      };
    }
  }
}
