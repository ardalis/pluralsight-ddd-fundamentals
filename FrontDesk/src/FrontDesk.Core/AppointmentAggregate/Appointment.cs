using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.AppointmentAggregate
{
  public class Appointment : BaseEntity<Guid>, IAggregateRoot
  {
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
      AppointmentTypeId = Guard.Against.NegativeOrZero(appointmentTypeId, nameof(appointmentTypeId));
      ScheduleId = Guard.Against.Default(scheduleId, nameof(scheduleId));
      ClientId = Guard.Against.NegativeOrZero(clientId, nameof(clientId));
      DoctorId = Guard.Against.NegativeOrZero(doctorId, nameof(doctorId));
      PatientId = Guard.Against.NegativeOrZero(patientId, nameof(patientId));
      RoomId = Guard.Against.NegativeOrZero(roomId, nameof(roomId));
      TimeRange = Guard.Against.Null(timeRange, nameof(timeRange));
      Title = Guard.Against.NullOrEmpty(title, nameof(title));
      DateTimeConfirmed = dateTimeConfirmed;
    }

    private Appointment() { } // EF required

    public int ClientId { get; private set; }
    public int PatientId { get; private set; }
    public int RoomId { get; private set; }
    public int DoctorId { get; private set; }
    public int AppointmentTypeId { get; private set; }

    public DateTimeOffsetRange TimeRange { get; private set; }
    public string Title { get; private set; }
    public DateTimeOffset? DateTimeConfirmed { get; set; }
    public bool IsPotentiallyConflicting { get; set; }

    public void UpdateRoom(int newRoomId)
    {
      Guard.Against.NegativeOrZero(newRoomId, nameof(newRoomId));
      if (newRoomId == RoomId) return;

      RoomId = newRoomId;

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void UpdateDoctor(int newDoctorId)
    {
      Guard.Against.NegativeOrZero(newDoctorId, nameof(newDoctorId));
      if (newDoctorId == DoctorId) return;

      DoctorId = newDoctorId;

      var appointmentUpdatedEvent = new AppointmentUpdatedEvent(this);
      Events.Add(appointmentUpdatedEvent);
    }

    public void UpdateStartTime(DateTimeOffset newStartTime, Action scheduleHandler)
    {
      if (newStartTime == TimeRange.Start) return;

      TimeRange = new DateTimeOffsetRange(newStartTime, TimeSpan.FromMinutes(TimeRange.DurationInMinutes()));

      scheduleHandler?.Invoke();

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

    private void MarkPotentiallyConflictingAppointments(List<Appointment> appointments)
    {
      // mark appointments that overlap as conflicting
      var foo = 0;
    }

    public async Task Schedule(IRepository<Appointment> appointmentRepository)
    {
      // Verify Appointment Fits in Schedule
      var spec = new AppointmentsOnDateSpec(TimeRange.Start.Date);
      var appointments = await appointmentRepository.ListAsync(spec);

      MarkPotentiallyConflictingAppointments(appointments);

      var appointmentScheduledEvent = new AppointmentScheduledEvent(this);
      Events.Add(appointmentScheduledEvent);

      // this will save any appointments with state changes (including those just marked as conflicting)
      await appointmentRepository.UpdateAsync(this);
    }
  }







  public class AppointmentsOnDateSpec : Specification<Appointment>
  {
    public AppointmentsOnDateSpec(DateTime startDate)
    {

    }
  }

}

