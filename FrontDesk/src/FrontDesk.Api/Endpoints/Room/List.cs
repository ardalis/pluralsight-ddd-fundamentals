using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Room;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using FrontDesk.Core.SyncedAggregates.Specifications;

namespace FrontDesk.Api.RoomEndpoints
{
  public class List : BaseAsyncEndpoint
    .WithRequest<ListRoomRequest>
    .WithResponse<ListRoomResponse>
  {
    private readonly IReadRepository<Room> _repository;
    private readonly IMapper _mapper;

    public List(IReadRepository<Room> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet(ListRoomRequest.Route)]
    [SwaggerOperation(
        Summary = "List Rooms",
        Description = "List Rooms",
        OperationId = "rooms.List",
        Tags = new[] { "RoomEndpoints" })
    ]
    public override async Task<ActionResult<ListRoomResponse>> HandleAsync([FromQuery] ListRoomRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListRoomResponse(request.CorrelationId());

      var roomSpec = new RoomSpecification();
      var rooms = await _repository.ListAsync(roomSpec);
      if (rooms is null) return NotFound();

      response.Rooms = _mapper.Map<List<RoomDto>>(rooms);
      response.Count = response.Rooms.Count;

      return Ok(response);
    }
  }
}
