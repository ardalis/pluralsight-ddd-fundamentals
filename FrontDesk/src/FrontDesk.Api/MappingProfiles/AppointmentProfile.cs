using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Api.MappingProfiles
{
  public class AppointmentProfile : Profile
  {
    public AppointmentProfile()
    {
      CreateMap<Appointment, AppointmentDto>()
          .ForMember(dto => dto.AppointmentId, options => options.MapFrom(src => src.Id))
          .ForMember(dto => dto.Start, options => options.MapFrom(src => src.TimeRange.Start))
          .ForMember(dto => dto.End, options => options.MapFrom(src => src.TimeRange.End))
          .ForMember(dto => dto.IsAllDay, options => options.MapFrom(src => false))
          .ForMember(dto => dto.IsConfirmed, options => options.MapFrom(src => src.DateTimeConfirmed.HasValue));
      CreateMap<AppointmentDto, Appointment>()
          .ForPath(dto => dto.TimeRange.Start, options => options.MapFrom(src => src.Start))
          .ForPath(dto => dto.TimeRange.End, options => options.MapFrom(src => src.End));
      CreateMap<CreateAppointmentRequest, Appointment>()
          .ForMember(dto => dto.DoctorId, options => options.MapFrom(src => src.SelectedDoctor))
          .ForPath(dto => dto.TimeRange.Start, options => options.MapFrom(src => src.DateOfAppointment))
          .ForPath(dto => dto.TimeRange.End, options => options.MapFrom(src => src.DateOfAppointment));
      CreateMap<UpdateAppointmentRequest, Appointment>()
          .ForPath(dto => dto.TimeRange.Start, options => options.MapFrom(src => src.Start));
      CreateMap<DeleteAppointmentRequest, Appointment>();
    }
  }
}
