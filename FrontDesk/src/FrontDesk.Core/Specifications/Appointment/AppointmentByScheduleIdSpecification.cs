﻿using System;
using Ardalis.Specification;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Core.Specifications
{
  public class AppointmentByScheduleIdSpecification : Specification<Appointment>
  {
    public AppointmentByScheduleIdSpecification(Guid scheduleId)
    {
      Query
          .Where(appointment => appointment.ScheduleId == scheduleId)
          .OrderBy(appointment => appointment.Title);
    }
  }
}
