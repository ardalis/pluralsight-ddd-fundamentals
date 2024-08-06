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
  /// Not Used.
  /// </summary>
  public class Delete : Endpoint<DeleteScheduleRequest, DeleteScheduleResponse>
  {
    private readonly IRepository<Schedule> _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository<Schedule> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Delete(DeleteScheduleRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Deletes a Schedule")
           .WithDescription("Deletes a Schedule")
           .WithName("schedules.delete")
           .WithTags("ScheduleEndpoints"));
    }

    public override async Task<DeleteScheduleResponse> ExecuteAsync(DeleteScheduleRequest request,
      CancellationToken cancellationToken)
    {
      var response = new DeleteScheduleResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Schedule>(request);
      await _repository.DeleteAsync(toDelete);

      return response;
    }
  }
}
