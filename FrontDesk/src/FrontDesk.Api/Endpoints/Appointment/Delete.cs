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
  public class Delete : BaseAsyncEndpoint<DeleteAppointmentRequest, DeleteAppointmentResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete("api/appointments/{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Appointment",
        Description = "Deletes a Appointment",
        OperationId = "appointments.delete",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<DeleteAppointmentResponse>> HandleAsync([FromRoute] DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteAppointmentResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Appointment>(request);
      await _repository.DeleteAsync<Appointment, Guid>(toDelete);

      return Ok(response);
    }
  }
}
