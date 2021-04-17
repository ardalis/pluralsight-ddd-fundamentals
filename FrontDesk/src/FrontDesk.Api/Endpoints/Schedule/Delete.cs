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
  /// <summary>
  /// Not Used.
  /// </summary>
  public class Delete : BaseAsyncEndpoint
    .WithRequest<DeleteScheduleRequest>
    .WithResponse<DeleteScheduleResponse>
  {
    private readonly IRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository<Schedule> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete(DeleteScheduleRequest.Route)]
    [SwaggerOperation(
        Summary = "Deletes a Schedule",
        Description = "Deletes a Schedule",
        OperationId = "schedules.delete",
        Tags = new[] { "ScheduleEndpoints" })
    ]
    public override async Task<ActionResult<DeleteScheduleResponse>> HandleAsync([FromRoute] DeleteScheduleRequest request,
      CancellationToken cancellationToken)
    {
      var response = new DeleteScheduleResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Schedule>(request);
      await _repository.DeleteAsync(toDelete);

      return Ok(response);
    }
  }
}
