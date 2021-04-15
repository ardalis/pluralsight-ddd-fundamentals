using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Schedule;
using FrontDesk.Core.ScheduleAggregate;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.ScheduleEndpoints
{
  public class List : BaseAsyncEndpoint
    .WithRequest<ListScheduleRequest>
    .WithResponse<ListScheduleResponse>
  {
    private readonly IReadRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public List(IReadRepository<Schedule> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet(ListScheduleRequest.Route)]
    [SwaggerOperation(
        Summary = "List Schedules",
        Description = "List Schedules",
        OperationId = "schedules.List",
        Tags = new[] { "ScheduleEndpoints" })
    ]
    public override async Task<ActionResult<ListScheduleResponse>> HandleAsync([FromQuery] ListScheduleRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListScheduleResponse(request.CorrelationId());

      var schedules = await _repository.ListAsync();
      if (schedules is null) return NotFound();

      response.Schedules = _mapper.Map<List<ScheduleDto>>(schedules);
      response.Count = response.Schedules.Count;

      return Ok(response);
    }
  }
}
