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
    public int? DoctorId { get; set; }
    public int AppointmentTypeId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public static UpdateAppointmentRequest FromDto(AppointmentDto appointmentDto)
    {
      return new UpdateAppointmentRequest()
      {
        Id = appointmentDto.AppointmentId,
        DoctorId = (int)appointmentDto.DoctorId,
        Title = appointmentDto.Title,
        ScheduleId = appointmentDto.ScheduleId,
        RoomId = appointmentDto.RoomId,
        AppointmentTypeId = appointmentDto.AppointmentTypeId,
        Start = appointmentDto.Start.DateTime,
        End = appointmentDto.End.DateTime,
      };
    }
  }
}
