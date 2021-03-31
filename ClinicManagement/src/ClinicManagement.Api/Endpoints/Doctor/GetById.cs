using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Doctor;
using ClinicManagement.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.DoctorEndpoints
{
  public class GetById : BaseAsyncEndpoint
    .WithRequest<GetByIdDoctorRequest>
    .WithResponse<GetByIdDoctorResponse>
  {
    private readonly IRepository<Doctor> _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository<Doctor> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/doctors/{DoctorId}")]
    [SwaggerOperation(
        Summary = "Get a Doctor by Id",
        Description = "Gets a Doctor by Id",
        OperationId = "doctors.GetById",
        Tags = new[] { "DoctorEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdDoctorResponse>> HandleAsync([FromRoute] GetByIdDoctorRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdDoctorResponse(request.CorrelationId);

      var doctor = await _repository.GetByIdAsync(request.DoctorId);
      if (doctor is null) return NotFound();

      response.Doctor = _mapper.Map<DoctorDto>(doctor);

      return Ok(response);
    }
  }


}
