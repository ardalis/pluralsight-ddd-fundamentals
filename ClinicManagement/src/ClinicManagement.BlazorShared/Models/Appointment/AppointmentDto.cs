using System;
using System.ComponentModel.DataAnnotations;
using BlazorShared.Models.AppointmentType;

namespace BlazorShared.Models.Appointment
{
  public class AppointmentDto
  {
    public Guid AppointmentId { get; set; }
    public Guid ScheduleId { get; set; }
    public int RoomId { get; set; }
    public int? DoctorId { get; set; }
    public int ClientId { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public string ClientName { get; set; }

    [Required(ErrorMessage = "The Start field is required")]
    public DateTime Start { get; set; }

    [Required(ErrorMessage = "The End field is required")]
    public DateTime End { get; set; }

    [Required(ErrorMessage = "The Title is required")]
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsAllDay { get; set; }
    public bool IsPotentiallyConflicting { get; set; }
    public bool IsConfirmed { get; set; }

    [Required(ErrorMessage = "The Appointment Type field is required")]
    public int AppointmentTypeId { get; set; }

    public AppointmentTypeDto AppointmentType { get; set; }

    public AppointmentDto ShallowCopy()
    {
      return (AppointmentDto)this.MemberwiseClone();
    }

    public override string ToString()
    {
      return $"Id: {AppointmentId} \nRoomId: {RoomId}\nDoctorId: {DoctorId}\nClient: {ClientId} {ClientName}\nPatient: {PatientId} {PatientName}\nStart: {Start}\nEnd:{End}";
    }
  }
}
