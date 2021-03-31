using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Room;
using ClinicManagement.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.RoomEndpoints
{
  public class GetById : BaseAsyncEndpoint
    .WithRequest<GetByIdRoomRequest>
    .WithResponse<GetByIdRoomResponse>
  {
    private readonly IRepository<Room> _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository<Room> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/rooms/{RoomId}")]
    [SwaggerOperation(
        Summary = "Get a Room by Id",
        Description = "Gets a Room by Id",
        OperationId = "rooms.GetById",
        Tags = new[] { "RoomEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdRoomResponse>> HandleAsync([FromRoute] GetByIdRoomRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdRoomResponse(request.CorrelationId);

      var room = await _repository.GetByIdAsync(request.RoomId);
      if (room is null) return NotFound();

      response.Room = _mapper.Map<RoomDto>(room);

      return Ok(response);
    }
  }


}
