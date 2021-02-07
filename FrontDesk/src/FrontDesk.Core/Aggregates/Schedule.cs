using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FrontDesk.Core.Events;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Aggregates
{
  public class Schedule : BaseEntity<Guid>, IAggregateRoot
  {
    public int ClinicId { get; private set; }

    // not persisted
    [NotMapped]
    public virtual DateTimeRange DateRange { get; private set; }

    private List<Appointment> _appointments;
    public IEnumerable<Appointment> Appointments
    {
      get
      {
        return _appointments.AsEnumerable();
      }
      private set
      {
        _appointments = (List<Appointment>)value;
      }
    }

    public Schedule(Guid id, DateTimeRange dateRange, int clinicId, IEnumerable<Appointment> appointments)
    {
      Id = id;
      DateRange = dateRange;
      ClinicId = clinicId;
      _appointments = appointments == null ? new List<Appointment>() : new List<Appointment>(appointments);
      MarkConflictingAppointments();
    }

    private Schedule() // required for EF
    {
      _appointments = new List<Appointment>();
    }

    public Appointment AddNewAppointment(Appointment appointment)
    {
      if (_appointments.Any(a => a.Id == appointment.Id))
      {
        throw new ArgumentException("Cannot add duplicate appointment to schedule.", nameof(appointment));
      }

      //appointment.State = TrackingState.Added;
      _appointments.Add(appointment);

      MarkConflictingAppointments();

      var appointmentScheduledEvent = new AppointmentScheduledEvent(appointment);

      return appointment;
    }

    public void DeleteAppointment(Appointment appointment)
    {
      // mark the appointment for deletion by the repository
      var appointmentToDelete = this.Appointments.Where(a => a.Id == appointment.Id).FirstOrDefault();
      if (appointmentToDelete != null)
      {
        //appointmentToDelete.State = TrackingState.Deleted;
      }

      MarkConflictingAppointments();
    }

    private void MarkConflictingAppointments()
    {
      foreach (var appointment in _appointments)
      {
        var potentiallyConflictingAppointments = _appointments
            .Where(a => a.PatientId == appointment.PatientId &&
            a.TimeRange.Overlaps(appointment.TimeRange) &&
            a.Id != appointment.Id).ToList();
        //  && a.State != TrackingState.Deleted).ToList();

        potentiallyConflictingAppointments.ForEach(a => a.IsPotentiallyConflicting = true);

        appointment.IsPotentiallyConflicting = potentiallyConflictingAppointments.Any();
      }
    }

    public void Handle(AppointmentUpdatedEvent args)
    {
      MarkConflictingAppointments();
    }
  }
}
