using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Room;
using ClinicManagement.Core.Aggregates;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.RoomEndpoints
{
  public class Update : Endpoint<UpdateRoomRequest, UpdateRoomResponse>
  {
    private readonly IRepository<Room> _repository;
    private readonly IMapper _mapper;

    public Update(IRepository<Room> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Put("api/rooms");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Updates a Room")
           .WithDescription("Updates a Room")
           .WithName("rooms.update")
           .WithTags("RoomEndpoints"));
    }

    public override async Task<UpdateRoomResponse> ExecuteAsync(UpdateRoomRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateRoomResponse(request.CorrelationId);

      var toUpdate = _mapper.Map<Room>(request);
      await _repository.UpdateAsync(toUpdate);

      var dto = _mapper.Map<RoomDto>(toUpdate);
      response.Room = dto;

      return response;
    }
  }
}
