﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class GetById : BaseAsyncEndpoint
    .WithRequest<GetByIdAppointmentRequest>
    .WithResponse<GetByIdAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IRepository<Client> _clientRepository;
    private readonly IMapper _mapper;

    public GetById(IRepository<Schedule> scheduleRepository,
      IRepository<Client> clientRepository,
      IMapper mapper)
    {
      _scheduleRepository = scheduleRepository;
      _clientRepository = clientRepository;
      _mapper = mapper;
    }

    [HttpGet(GetByIdAppointmentRequest.Route)]
    [SwaggerOperation(
        Summary = "Get a Appointment by Id",
        Description = "Gets a Appointment by Id",
        OperationId = "appointments.GetById",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdAppointmentResponse>> HandleAsync([FromRoute] GetByIdAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      var appointment = schedule.Appointments.FirstOrDefault(a => a.Id == request.AppointmentId);
      if (appointment == null) return NotFound();

      response.Appointment = _mapper.Map<AppointmentDto>(appointment);

      // load names
      var clientSpec = new ClientByIdIncludePatientsSpecification(appointment.ClientId);
      var client = await _clientRepository.GetBySpecAsync(clientSpec);
      var patient = client.Patients.First(p => p.Id == appointment.PatientId);

      response.Appointment.ClientName = client.FullName;
      response.Appointment.PatientName = patient.Name;

      return Ok(response);
    }
  }


}
