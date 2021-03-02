using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Exceptions;
using FrontDesk.Core.Interfaces;
using FrontDesk.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class List : BaseAsyncEndpoint
    .WithRequest<ListAppointmentRequest>
    .WithResponse<ListAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IMapper _mapper;
    private readonly IApplicationSettings _settings;

    public List(IRepository<Schedule> scheduleRepository,
      IMapper mapper,
      IApplicationSettings settings)
    {
      _scheduleRepository = scheduleRepository;
      _mapper = mapper;
      _settings = settings;
    }

    [HttpGet(ListAppointmentRequest.Route)]
    [SwaggerOperation(
        Summary = "List Appointments",
        Description = "List Appointments",
        OperationId = "appointments.List",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<ListAppointmentResponse>> HandleAsync([FromQuery] ListAppointmentRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListAppointmentResponse(request.CorrelationId());


      //var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      
      var spec = new ScheduleForClinicAndDateWithAppointmentsSpec(_settings.ClinicId, _settings.TestDate);
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      if (schedule == null) throw new ScheduleNotFoundException($"No schedule found for clinic {_settings.ClinicId}.");

      var myAppointments = _mapper.Map<List<AppointmentDto>>(schedule.Appointments);

      response.Appointments = myAppointments.OrderBy(a => a.Start).ToList();
      response.Count = response.Appointments.Count;

      return Ok(response);
    }
  }
}
