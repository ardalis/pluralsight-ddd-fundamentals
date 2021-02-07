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
  public class Delete : BaseAsyncEndpoint<DeleteScheduleRequest, DeleteScheduleResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete("api/schedules/{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Schedule",
        Description = "Deletes a Schedule",
        OperationId = "schedules.delete",
        Tags = new[] { "ScheduleEndpoints" })
    ]
    public override async Task<ActionResult<DeleteScheduleResponse>> HandleAsync([FromRoute] DeleteScheduleRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteScheduleResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Schedule>(request);
      await _repository.DeleteAsync<Schedule, Guid>(toDelete);

      return Ok(response);
    }
  }
}
