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
    private readonly IRepository<Client> _clientRepository;
    private readonly IMapper _mapper;
    private readonly IApplicationSettings _settings;

    public List(IRepository<Schedule> scheduleRepository,
      IRepository<Client> clientRepository,
      IMapper mapper,
      IApplicationSettings settings)
    {
      _scheduleRepository = scheduleRepository;
      _clientRepository = clientRepository;
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
      Schedule schedule = null;
      if (request.ScheduleId == Guid.Empty)
      {
        var spec = new ScheduleForClinicAndDateWithAppointmentsSpec(_settings.ClinicId, _settings.TestDate);
        schedule = await _scheduleRepository.GetBySpecAsync(spec);
        if (schedule == null) throw new ScheduleNotFoundException($"No schedule found for clinic {_settings.ClinicId}.");
      }
      else
      {
        var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId);
        schedule = await _scheduleRepository.GetBySpecAsync(spec);
        if (schedule == null) throw new ScheduleNotFoundException($"No schedule found for id {request.ScheduleId}.");
      }
      var myAppointments = _mapper.Map<List<AppointmentDto>>(schedule.Appointments);

      // load names - only do this kind of thing if you have caching!
      // N+1 query problem
      // Possibly use custom SQL or view or stored procedure instead
      foreach (var appt in myAppointments)
      {
        var clientSpec = new ClientByIdIncludePatientsSpecification(appt.ClientId);
        var client = await _clientRepository.GetBySpecAsync(clientSpec);
        var patient = client.Patients.First(p => p.Id == appt.PatientId);

        appt.ClientName = client.FullName;
        appt.PatientName = patient.Name;
      }

      response.Appointments = myAppointments.OrderBy(a => a.Start).ToList();
      response.Count = response.Appointments.Count;

      return Ok(response);
    }

  //  public override async Task<ActionResult<ListAppointmentResponse>> HandleAsync([FromQuery] ListAppointmentRequest request,
  //CancellationToken cancellationToken)
  //  {
  //    var response = new ListAppointmentResponse(request.CorrelationId());
  //    Schedule schedule = null;
  //    if (request.ScheduleId == Guid.Empty)
  //    {
  //      var spec = new ScheduleForClinicAndDateWithAppointmentsSpec(_settings.ClinicId, _settings.TestDate);
  //      schedule = await _scheduleRepository.GetBySpecAsync(spec);
  //      if (schedule == null) throw new ScheduleNotFoundException($"No schedule found for clinic {_settings.ClinicId}.");
  //    }
  //    else
  //    {
  //      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId);
  //      schedule = await _scheduleRepository.GetBySpecAsync(spec);
  //      if (schedule == null) throw new ScheduleNotFoundException($"No schedule found for id {request.ScheduleId}.");
  //    }
  //    schedule.Handle(null); // mark conflicts
  //    var myAppointments = _mapper.Map<List<AppointmentDto>>(schedule.Appointments);

  //    // load names - only do this kind of thing if you have caching!
  //    // N+1 query problem
  //    // Possibly use custom SQL or view or stored procedure instead
  //    foreach (var appt in myAppointments)
  //    {
  //      var clientSpec = new ClientByIdIncludePatientsSpecification(appt.ClientId);
  //      var client = await _clientRepository.GetBySpecAsync(clientSpec);
  //      var patient = client.Patients.First(p => p.Id == appt.PatientId);

  //      appt.ClientName = client.FullName;
  //      appt.PatientName = patient.Name;
  //    }

  //    response.Appointments = myAppointments.OrderBy(a => a.Start).ToList();
  //    response.Count = response.Appointments.Count;

  //    return Ok(response);
  //  }

  }
}
