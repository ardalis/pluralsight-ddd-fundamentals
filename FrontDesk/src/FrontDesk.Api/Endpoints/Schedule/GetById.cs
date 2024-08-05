using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Schedule;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.ScheduleEndpoints
{
  public class GetById : Endpoint<GetByIdScheduleRequest, Results<Ok<GetByIdScheduleResponse>, NotFound>>
  {
    private readonly IReadRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Schedule> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get("api/schedules/{ScheduleId}");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Get a Schedule by Id")
           .WithDescription("Gets a Schedule by Id")
           .WithName("schedules.GetById")
           .WithTags("ScheduleEndpoints"));
    }

    public override async Task<Results<Ok<GetByIdScheduleResponse>, NotFound>> ExecuteAsync([FromRoute] GetByIdScheduleRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdScheduleResponse(request.CorrelationId());

      var schedule = await _repository.GetByIdAsync(request.ScheduleId);
      if (schedule is null) return TypedResults.NotFound();

      response.Schedule = _mapper.Map<ScheduleDto>(schedule);

      return TypedResults.Ok(response);
    }
  }
}
