using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class List : BaseAsyncEndpoint<ListAppointmentRequest, ListAppointmentResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly IApplicationSettings _settings;

    public List(IRepository repository, IMapper mapper, IApplicationSettings settings)
    {
      _repository = repository;
      _mapper = mapper;
      _settings = settings;
    }

    [HttpGet("api/appointments")]
    [SwaggerOperation(
        Summary = "List Appointments",
        Description = "List Appointments",
        OperationId = "appointments.List",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<ListAppointmentResponse>> HandleAsync([FromQuery] ListAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new ListAppointmentResponse(request.CorrelationId());

      var scheduleSpec = new ScheduleForDateAndClinicSpecification(_settings.ClinicId, _settings.TestDate);

      int totalSchedules = await _repository.CountAsync<Schedule, Guid>(scheduleSpec);
      if (totalSchedules <= 0)
      {
        response.Appointments = new List<AppointmentDto>();
        response.Count = 0;
        return Ok(response);
      }

      var schedule = (await _repository.ListAsync<Schedule, Guid>(scheduleSpec)).First();

      var appointmentSpec = new AppointmentByScheduleIdSpecification(schedule.Id);
      var appointments = (await _repository.ListAsync<Appointment, Guid>(appointmentSpec)).ToList();

      var myAppointments = _mapper.Map<List<AppointmentDto>>(appointments);

      response.Appointments = myAppointments.OrderBy(a => a.Start).ToList();
      response.Count = response.Appointments.Count;

      return Ok(response);
    }
  }
}