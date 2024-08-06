using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Schedule;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.ScheduleEndpoints
{
  public class List : Endpoint<ListScheduleRequest, Results<Ok<ListScheduleResponse>, NotFound>>
  {
    private readonly IReadRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public List(IReadRepository<Schedule> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get(ListScheduleRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("List Schedules")
           .WithDescription("List Schedules")
           .WithName("schedules.List")
           .WithTags("ScheduleEndpoints"));
    }

    public override async Task<Results<Ok<ListScheduleResponse>, NotFound>> ExecuteAsync(ListScheduleRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListScheduleResponse(request.CorrelationId());

      var schedules = await _repository.ListAsync();
      if (schedules is null) return TypedResults.NotFound();

      response.Schedules = _mapper.Map<List<ScheduleDto>>(schedules);
      response.Count = response.Schedules.Count;

      return TypedResults.Ok(response);
    }
  }
}
