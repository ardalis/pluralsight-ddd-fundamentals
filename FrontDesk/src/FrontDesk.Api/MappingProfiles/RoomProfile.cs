using AutoMapper;
using BlazorShared.Models.Room;
using FrontDesk.Core.SyncedAggregates;

namespace FrontDesk.Api.MappingProfiles
{
  public class RoomProfile : Profile
  {
    public RoomProfile()
    {
      CreateMap<Room, RoomDto>()
          .ForMember(dto => dto.RoomId, options => options.MapFrom(src => src.Id));
      CreateMap<RoomDto, Room>()
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.RoomId));
      CreateMap<CreateRoomRequest, Room>();
      CreateMap<UpdateRoomRequest, Room>()
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.RoomId));
      CreateMap<DeleteRoomRequest, Room>();
    }
  }
}
