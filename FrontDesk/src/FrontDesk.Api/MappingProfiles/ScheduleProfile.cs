using AutoMapper;
using BlazorShared.Models.Schedule;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Api.MappingProfiles
{
  public class ScheduleProfile : Profile
  {
    public ScheduleProfile()
    {
      CreateMap<Schedule, ScheduleDto>();
      CreateMap<ScheduleDto, Schedule>();
      CreateMap<CreateScheduleRequest, Schedule>();
      CreateMap<UpdateScheduleRequest, Schedule>();
      CreateMap<DeleteScheduleRequest, Schedule>();
    }
  }
}
