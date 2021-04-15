using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.AppointmentType;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentTypeEndpoints
{
  public class List : BaseAsyncEndpoint
    .WithRequest<ListAppointmentTypeRequest>
    .WithResponse<ListAppointmentTypeResponse>
  {
    private readonly IReadRepository<AppointmentType> _appointmentTypeRepository;
    private readonly IMapper _mapper;

    public List(IReadRepository<AppointmentType> appointmentTypeRepository, IMapper mapper)
    {
      _appointmentTypeRepository = appointmentTypeRepository;
      _mapper = mapper;
    }

    [HttpGet(ListAppointmentTypeRequest.Route)]
    [SwaggerOperation(
        Summary = "List Appointment Types",
        Description = "List Appointment Types",
        OperationId = "appointment-types.List",
        Tags = new[] { "AppointmentTypeEndpoints" })
    ]
    public override async Task<ActionResult<ListAppointmentTypeResponse>> HandleAsync([FromQuery] ListAppointmentTypeRequest request, CancellationToken cancellationToken)
    {
      var response = new ListAppointmentTypeResponse(request.CorrelationId());

      var appointmentTypes = await _appointmentTypeRepository.ListAsync();
      response.AppointmentTypes = _mapper.Map<List<AppointmentTypeDto>>(appointmentTypes);
      response.Count = response.AppointmentTypes.Count;

      return Ok(response);
    }
  }
}
