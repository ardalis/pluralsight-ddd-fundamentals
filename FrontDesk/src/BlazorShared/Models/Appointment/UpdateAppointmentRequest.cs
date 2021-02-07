using System;

namespace BlazorShared.Models.Appointment
{
  public class UpdateAppointmentRequest : BaseRequest
  {
    public Guid Id { get; set; }
    public Guid ScheduleId { get; set; }
    public string Title { get; set; }
    public int ClientId { get; set; }
    public int PatientId { get; set; }
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
        PatientId = appointmentDto.PatientId,
        Title = appointmentDto.Title,
        ClientId = appointmentDto.ClientId,
        ScheduleId = appointmentDto.ScheduleId,
        RoomId = appointmentDto.RoomId,
        AppointmentTypeId = appointmentDto.AppointmentTypeId,
        Start = appointmentDto.Start,
        End = appointmentDto.End,
      };
    }

  }
}
