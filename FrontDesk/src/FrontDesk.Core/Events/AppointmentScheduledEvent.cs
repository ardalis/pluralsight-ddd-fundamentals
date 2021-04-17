﻿using System;
using FrontDesk.Core.ScheduleAggregate;
using PluralsightDdd.SharedKernel;

namespace FrontDesk.Core.Events
{
  public class AppointmentScheduledEvent : BaseDomainEvent
  {
    public AppointmentScheduledEvent(Appointment appointment) : this()
    {
      AppointmentScheduled = appointment;
    }

    public AppointmentScheduledEvent()
    {
      this.Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }

    public Appointment AppointmentScheduled { get; private set; }
  }
}
