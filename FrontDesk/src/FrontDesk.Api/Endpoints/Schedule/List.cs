using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Schedule;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.ScheduleEndpoints
{
  public class List : BaseAsyncEndpoint<ListScheduleRequest, ListScheduleResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public List(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/schedules")]
    [SwaggerOperation(
        Summary = "List Schedules",
        Description = "List Schedules",
        OperationId = "schedules.List",
        Tags = new[] { "ScheduleEndpoints" })
    ]
    public override async Task<ActionResult<ListScheduleResponse>> HandleAsync([FromQuery] ListScheduleRequest request, CancellationToken cancellationToken)
    {
      var response = new ListScheduleResponse(request.CorrelationId());

      var schedules = await _repository.ListAsync<Schedule, Guid>();
      if (schedules is null) return NotFound();

      response.Schedules = _mapper.Map<List<ScheduleDto>>(schedules);
      response.Count = response.Schedules.Count;

      return Ok(response);
    }
  }
}