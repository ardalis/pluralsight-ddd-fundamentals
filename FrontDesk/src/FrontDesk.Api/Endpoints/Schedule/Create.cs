using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Schedule;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.ScheduleEndpoints
{
  /// <summary>
  /// Not used.
  /// </summary>
  public class Create : Endpoint<CreateScheduleRequest, CreateScheduleResponse>
  {
    private readonly IRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public Create(IRepository<Schedule> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Post("api/schedules");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Creates a new Schedule")
           .WithDescription("Creates a new Schedule")
           .WithName("schedules.create")
           .WithTags("ScheduleEndpoints"));
    }

    public override async Task<CreateScheduleResponse> ExecuteAsync(CreateScheduleRequest request,
      CancellationToken cancellationToken)
    {
      var response = new CreateScheduleResponse(request.CorrelationId());

      var toAdd = _mapper.Map<Schedule>(request);
      toAdd = await _repository.AddAsync(toAdd);

      var dto = _mapper.Map<ScheduleDto>(toAdd);
      response.Schedule = dto;

      return response;
    }
  }
}
