using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Create : BaseAsyncEndpoint<CreateAppointmentRequest, CreateAppointmentResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Create(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPost("api/appointments")]
    [SwaggerOperation(
        Summary = "Creates a new Appointment",
        Description = "Creates a new Appointment",
        OperationId = "appointments.create",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<CreateAppointmentResponse>> HandleAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new CreateAppointmentResponse(request.CorrelationId());

      var toAdd = _mapper.Map<Appointment>(request);

      var appointmentType = await _repository.GetByIdAsync<AppointmentType, int>(toAdd.AppointmentTypeId);
      toAdd.UpdateEndTime(appointmentType);

      toAdd = await _repository.AddAsync<Appointment, Guid>(toAdd);

      var dto = _mapper.Map<AppointmentDto>(toAdd);
      response.Appointment = dto;

      return Ok(response);
    }
  }
}
