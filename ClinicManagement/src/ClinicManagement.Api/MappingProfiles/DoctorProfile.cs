using AutoMapper;
using BlazorShared.Models.Doctor;
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
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.DoctorId));
      CreateMap<CreateDoctorRequest, Doctor>();
      CreateMap<UpdateDoctorRequest, Doctor>()
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.DoctorId));
      CreateMap<DeleteDoctorRequest, Doctor>();
    }
  }
}
