using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Room;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.RoomEndpoints
{
  public class List : Endpoint<ListRoomRequest, Results<Ok<ListRoomResponse>, NotFound>>
  {
    private readonly IRepository<Room> _repository;
    private readonly IMapper _mapper;

    public List(IRepository<Room> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get("api/rooms");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("List Rooms")
           .WithDescription("List Rooms")
           .WithName("rooms.List")
           .WithTags("RoomEndpoints"));
    }

    public override async Task<Results<Ok<ListRoomResponse>, NotFound>> ExecuteAsync(ListRoomRequest request, CancellationToken cancellationToken)
    {
      var response = new ListRoomResponse(request.CorrelationId);

      var roomSpec = new RoomSpecification();
      var rooms = await _repository.ListAsync(roomSpec);
      if (rooms is null) return TypedResults.NotFound();

      response.Rooms = _mapper.Map<List<RoomDto>>(rooms);
      response.Count = response.Rooms.Count;

      return TypedResults.Ok(response);
    }
  }
}
