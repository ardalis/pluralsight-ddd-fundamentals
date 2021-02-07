using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Doctor;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.DoctorEndpoints
{
  public class Create : BaseAsyncEndpoint<CreateDoctorRequest, CreateDoctorResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Create(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPost("api/doctors")]
    [SwaggerOperation(
        Summary = "Creates a new Doctor",
        Description = "Creates a new Doctor",
        OperationId = "doctors.create",
        Tags = new[] { "DoctorEndpoints" })
    ]
    public override async Task<ActionResult<CreateDoctorResponse>> HandleAsync(CreateDoctorRequest request, CancellationToken cancellationToken)
    {
      var response = new CreateDoctorResponse(request.CorrelationId());

      var toAdd = _mapper.Map<Doctor>(request);
      toAdd = await _repository.AddAsync<Doctor, int>(toAdd);

      var dto = _mapper.Map<DoctorDto>(toAdd);
      response.Doctor = dto;

      return Ok(response);
    }
  }
}
