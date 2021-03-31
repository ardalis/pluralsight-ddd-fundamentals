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
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreateRoomRequest>
    .WithResponse<CreateRoomResponse>
  {
    private readonly IRepository<Room> _repository;
    private readonly IMapper _mapper;

    public Create(IRepository<Room> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPost("api/rooms")]
    [SwaggerOperation(
        Summary = "Creates a new Room",
        Description = "Creates a new Room",
        OperationId = "rooms.create",
        Tags = new[] { "RoomEndpoints" })
    ]
    public override async Task<ActionResult<CreateRoomResponse>> HandleAsync(CreateRoomRequest request, CancellationToken cancellationToken)
    {
      var response = new CreateRoomResponse(request.CorrelationId);

      var toAdd = _mapper.Map<Room>(request);
      toAdd = await _repository.AddAsync(toAdd);

      var dto = _mapper.Map<RoomDto>(toAdd);
      response.Room = dto;

      return Ok(response);
    }
  }
}
