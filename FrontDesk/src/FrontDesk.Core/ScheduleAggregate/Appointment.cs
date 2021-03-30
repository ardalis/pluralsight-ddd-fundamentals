using System;
using Ardalis.GuardClauses;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Aggregates
{
  public class Appointment : BaseEntity<Guid>
  {
    public Guid ScheduleId { get; private set; }
    public int ClientId { get; private set; }
    public int PatientId { get; private set; }
    public int RoomId { get; private set; }
    public int DoctorId { get; private set; }
    public int AppointmentTypeId { get; private set; }

    public DateTimeOffsetRange TimeRange { get; private set; }
    public string Title { get; private set; }
    public DateTimeOffset? DateTimeConfirmed { get; set; }
    public bool IsPotentiallyConflicting { get; set; }

    // EF https://github.com/dotnet/efcore/issues/12078#issuecomment-498379223
    public Appointment(int appointmentTypeId,
      Guid scheduleId,
      int clientId,
      int doctorId,
      int patientId,
      int roomId,
      DateTimeOffsetRange timeRange, // EF Core 5 cannot provide this type
      string title,
      DateTime? dateTimeConfirmed = null)
    {
      Guard.Against.NegativeOrZero(appointmentTypeId, nameof(appointmentTypeId));
      Guard.Against.Default(scheduleId, nameof(scheduleId));
      Guard.Against.NegativeOrZero(clientId, nameof(clientId));
      Guard.Against.NegativeOrZero(doctorId, nameof(doctorId));
      Guard.Against.NegativeOrZero(patientId, nameof(patientId));
      Guard.Against.NegativeOrZero(roomId, nameof(roomId));
      Guard.Against.NullOrEmpty(title, nameof(title));
      AppointmentTypeId = appointmentTypeId;
      ScheduleId = scheduleId;
      ClientId = clientId;
      DoctorId = doctorId;
      PatientId = patientId;
      RoomId = roomId;
      TimeRange = timeRange;
      Title = title;
      DateTimeConfirmed = dateTimeConfirmed;
    }

    private Appointment() { } // EF required

    public void UpdateRoom(int newRoomId)
    {
      if (newRoomId == RoomId) return;

      RoomId = newRoomId;

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void UpdateDoctor(int newDoctorId)
    {
      if (newDoctorId == DoctorId) return;

      DoctorId = newDoctorId;

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void UpdateStartTime(DateTimeOffset newStartTime)
    {
      if (newStartTime == TimeRange.Start) return;

      TimeRange = new DateTimeOffsetRange(newStartTime, TimeSpan.FromMinutes(TimeRange.DurationInMinutes()));

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void UpdateTitle(string newTitle)
    {
      if (newTitle == Title) return;

      Title = newTitle;

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void UpdateAppointmentType(AppointmentType appointmentType)
    {
      Guard.Against.Null(appointmentType, nameof(appointmentType));
      if (AppointmentTypeId == appointmentType.Id) return;

      AppointmentTypeId = appointmentType.Id;
      TimeRange = TimeRange.NewEnd(TimeRange.Start.AddMinutes(appointmentType.Duration));

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void Confirm(DateTimeOffset dateConfirmed)
    {
      if (DateTimeConfirmed.HasValue) return; // no need to reconfirm

      DateTimeConfirmed = dateConfirmed;

      var appointmentConfirmedEvent = new AppointmentConfirmedEvent(this);
      Events.Add(appointmentConfirmedEvent);
    }
  }
}
