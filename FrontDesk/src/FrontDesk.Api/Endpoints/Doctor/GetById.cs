using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Doctor;
using FastEndpoints;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.DoctorEndpoints
{
  public class GetById : Endpoint<GetByIdDoctorRequest, Results<Ok<GetByIdDoctorResponse>, NotFound>>
  {
    private readonly IReadRepository<Doctor> _repository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Doctor> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get("api/doctors/{DoctorId}");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Get a Doctor by Id")
           .WithDescription("Gets a Doctor by Id")
           .WithName("doctors.GetById")
           .WithTags("DoctorEndpoints"));
    }

    public override async Task<Results<Ok<GetByIdDoctorResponse>, NotFound>> ExecuteAsync(GetByIdDoctorRequest request,
      CancellationToken cancellationToken)
    {
      var response = new GetByIdDoctorResponse(request.CorrelationId());

      var doctor = await _repository.GetByIdAsync(request.DoctorId);
      if (doctor is null) return TypedResults.NotFound();

      response.Doctor = _mapper.Map<DoctorDto>(doctor);

      return TypedResults.Ok(response);
    }
  }
}
