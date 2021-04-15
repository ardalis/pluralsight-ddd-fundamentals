using System;
using System.Collections.Generic;
using System.Linq;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.ScheduleAggregate
{
  public class Schedule : BaseEntity<Guid>, IAggregateRoot
  {
    public int ClinicId { get; private set; }
    private readonly List<Appointment> _appointments = new List<Appointment>();
    public IEnumerable<Appointment> Appointments => _appointments.AsReadOnly();

    public DateTimeOffsetRange DateRange { get; private set; }

    public Schedule(Guid id,
      DateTimeOffsetRange dateRange,
      int clinicId,
      IEnumerable<Appointment> appointments)
    {
      Id = id;
      DateRange = dateRange;
      ClinicId = clinicId;
      _appointments.AddRange(appointments ?? new List<Appointment>());
      MarkConflictingAppointments();
    }

    public Schedule(Guid id,
      int clinicId)
    {
      Id = id;
      ClinicId = clinicId;
    }

    public Appointment AddNewAppointment(Appointment appointment)
    {
      if (appointment.Id != Guid.Empty &&
        _appointments.Any(a => a.Id == appointment.Id))
      {
        throw new ArgumentException("Cannot add duplicate appointment to schedule.", nameof(appointment));
      }

      _appointments.Add(appointment);

      MarkConflictingAppointments();

      var appointmentScheduledEvent = new AppointmentScheduledEvent(appointment);
      Events.Add(appointmentScheduledEvent);

      return appointment;
    }

    public void DeleteAppointment(Appointment appointment)
    {
      var appointmentToDelete = _appointments
                                .Where(a => a.Id == appointment.Id)
                                .FirstOrDefault();

      if (appointmentToDelete != null)
      {
        _appointments.Remove(appointmentToDelete);
      }

      MarkConflictingAppointments();

      // TODO: Add appointment deleted event and show delete message in Blazor client app
    }

    private void MarkConflictingAppointments()
    {
      foreach (var appointment in _appointments)
      {
        // same patient cannot have two appointments at same time
        var potentiallyConflictingAppointments = _appointments
            .Where(a => a.PatientId == appointment.PatientId &&
            a.TimeRange.Overlaps(appointment.TimeRange) &&
            a != appointment)
            .ToList();

        // TODO: Add a rule to mark overlapping appointments in same room as conflicting
        // TODO: Add a rule to mark same doctor with overlapping appointments as conflicting

        potentiallyConflictingAppointments.ForEach(a => a.IsPotentiallyConflicting = true);

        appointment.IsPotentiallyConflicting = potentiallyConflictingAppointments.Any();
      }
    }

    /// <summary>
    /// Call any time this schedule's appointments are updated directly
    /// </summary>
    public void AppointmentUpdatedHandler()
    {
      // TODO: Add ScheduleHandler calls to UpdateDoctor, UpdateRoom to complete additional rules described in MarkConflictingAppointments
      MarkConflictingAppointments();
    }
  }
}
