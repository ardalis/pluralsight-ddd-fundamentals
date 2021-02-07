using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Room;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.RoomEndpoints
{
  public class Update : BaseAsyncEndpoint<UpdateRoomRequest, UpdateRoomResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Update(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPut("api/rooms")]
    [SwaggerOperation(
        Summary = "Updates a Room",
        Description = "Updates a Room",
        OperationId = "rooms.update",
        Tags = new[] { "RoomEndpoints" })
    ]
    public override async Task<ActionResult<UpdateRoomResponse>> HandleAsync(UpdateRoomRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateRoomResponse(request.CorrelationId());

      var toUpdate = _mapper.Map<Room>(request);
      await _repository.UpdateAsync<Room, int>(toUpdate);

      var dto = _mapper.Map<RoomDto>(toUpdate);
      response.Room = dto;

      return Ok(response);
    }
  }
}
