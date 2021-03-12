using AutoMapper;
using BlazorShared.Models.Doctor;
using ClinicManagement.Api.ApplicationEvents;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Api.MappingProfiles
{
  public class DoctorProfile : Profile
  {
    public DoctorProfile()
    {
      CreateMap<Doctor, DoctorDto>()
          .ForMember(dto => dto.DoctorId, options => options.MapFrom(src => src.Id));
      CreateMap<DoctorDto, Doctor>()
          .ConstructUsing(dto => new Doctor(dto.DoctorId, dto.Name));
      CreateMap<CreateDoctorRequest, Doctor>()
          .ConstructUsing(dto => new Doctor(0, dto.Name));
      CreateMap<UpdateDoctorRequest, Doctor>()
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.DoctorId));
      CreateMap<DeleteDoctorRequest, Doctor>();
      CreateMap<Doctor, NamedEntity>();
    }
  }
}
