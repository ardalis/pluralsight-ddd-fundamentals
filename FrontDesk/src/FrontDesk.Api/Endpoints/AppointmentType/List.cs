using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.AppointmentType;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentTypeEndpoints
{
  public class List : BaseAsyncEndpoint<ListAppointmentTypeRequest, ListAppointmentTypeResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public List(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/appointment-types")]
    [SwaggerOperation(
        Summary = "List Appointment Types",
        Description = "List Appointment Types",
        OperationId = "appointment-types.List",
        Tags = new[] { "AppointmentTypeEndpoints" })
    ]
    public override async Task<ActionResult<ListAppointmentTypeResponse>> HandleAsync([FromQuery] ListAppointmentTypeRequest request, CancellationToken cancellationToken)
    {
      var response = new ListAppointmentTypeResponse(request.CorrelationId());

      var appointmentTypes = await _repository.ListAsync<AppointmentType, int>();
      response.AppointmentTypes = _mapper.Map<List<AppointmentTypeDto>>(appointmentTypes);
      response.Count = response.AppointmentTypes.Count;

      return Ok(response);
    }
  }
}