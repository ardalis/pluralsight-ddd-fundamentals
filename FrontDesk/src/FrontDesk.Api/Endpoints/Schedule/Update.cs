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
  public class Update : Endpoint<UpdateScheduleRequest, UpdateScheduleResponse>
  {
    private readonly IRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public Update(IRepository<Schedule> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Put("api/schedules");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Updates a Schedule")
           .WithDescription("Updates a Schedule")
           .WithName("schedules.update")
           .WithTags("ScheduleEndpoints"));
    }

    public override async Task<UpdateScheduleResponse> ExecuteAsync(UpdateScheduleRequest request,
      CancellationToken cancellationToken)
    {
      var response = new UpdateScheduleResponse(request.CorrelationId());

      var toUpdate = _mapper.Map<Schedule>(request);
      await _repository.UpdateAsync(toUpdate);

      var dto = _mapper.Map<ScheduleDto>(toUpdate);
      response.Schedule = dto;

      return response;
    }
  }
}
