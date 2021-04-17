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
  public class GetById : BaseAsyncEndpoint
    .WithRequest<GetByIdScheduleRequest>
    .WithResponse<GetByIdScheduleResponse>
  {
    private readonly IReadRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Schedule> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/schedules/{ScheduleId}")]
    [SwaggerOperation(
        Summary = "Get a Schedule by Id",
        Description = "Gets a Schedule by Id",
        OperationId = "schedules.GetById",
        Tags = new[] { "ScheduleEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdScheduleResponse>> HandleAsync([FromRoute] GetByIdScheduleRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdScheduleResponse(request.CorrelationId());

      var schedule = await _repository.GetByIdAsync(request.ScheduleId);
      if (schedule is null) return NotFound();

      response.Schedule = _mapper.Map<ScheduleDto>(schedule);

      return Ok(response);
    }
  }


}
