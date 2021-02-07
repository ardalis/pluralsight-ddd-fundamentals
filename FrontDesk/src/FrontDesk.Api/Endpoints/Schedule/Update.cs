using System;
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
  public class Update : BaseAsyncEndpoint<UpdateScheduleRequest, UpdateScheduleResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Update(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPut("api/schedules")]
    [SwaggerOperation(
        Summary = "Updates a Schedule",
        Description = "Updates a Schedule",
        OperationId = "schedules.update",
        Tags = new[] { "ScheduleEndpoints" })
    ]
    public override async Task<ActionResult<UpdateScheduleResponse>> HandleAsync(UpdateScheduleRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateScheduleResponse(request.CorrelationId());

      var toUpdate = _mapper.Map<Schedule>(request);
      await _repository.UpdateAsync<Schedule, Guid>(toUpdate);

      var dto = _mapper.Map<ScheduleDto>(toUpdate);
      response.Schedule = dto;

      return Ok(response);
    }
  }
}
