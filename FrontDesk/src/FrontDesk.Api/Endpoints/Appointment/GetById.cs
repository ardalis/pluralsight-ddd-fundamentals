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
  public class GetById : BaseAsyncEndpoint<GetByIdAppointmentRequest, GetByIdAppointmentResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/appointments/{AppointmentId}")]
    [SwaggerOperation(
        Summary = "Get a Appointment by Id",
        Description = "Gets a Appointment by Id",
        OperationId = "appointments.GetById",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdAppointmentResponse>> HandleAsync([FromRoute] GetByIdAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdAppointmentResponse(request.CorrelationId());

      var appointment = await _repository.GetByIdAsync<Appointment, Guid>(request.AppointmentId);
      if (appointment is null) return NotFound();

      response.Appointment = _mapper.Map<AppointmentDto>(appointment);

      return Ok(response);
    }
  }


}
