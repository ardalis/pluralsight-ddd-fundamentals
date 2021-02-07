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
  public class Update : BaseAsyncEndpoint<UpdateAppointmentRequest, UpdateAppointmentResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Update(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPut("api/appointments")]
    [SwaggerOperation(
        Summary = "Updates a Appointment",
        Description = "Updates a Appointment",
        OperationId = "appointments.update",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<UpdateAppointmentResponse>> HandleAsync(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateAppointmentResponse(request.CorrelationId());

      var toUpdate = _mapper.Map<Appointment>(request);

      var appointmentType = await _repository.GetByIdAsync<AppointmentType, int>(toUpdate.AppointmentTypeId);
      toUpdate.UpdateEndTime(appointmentType);

      await _repository.UpdateAsync<Appointment, Guid>(toUpdate);

      var dto = _mapper.Map<AppointmentDto>(toUpdate);
      response.Appointment = dto;

      return Ok(response);
    }
  }
}
