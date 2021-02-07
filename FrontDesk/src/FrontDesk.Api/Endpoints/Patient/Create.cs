using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Patient;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.PatientEndpoints
{
  public class Create : BaseAsyncEndpoint<CreatePatientRequest, CreatePatientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Create(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPost("api/patients")]
    [SwaggerOperation(
        Summary = "Creates a new Patient",
        Description = "Creates a new Patient",
        OperationId = "patients.create",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<CreatePatientResponse>> HandleAsync(CreatePatientRequest request, CancellationToken cancellationToken)
    {
      var response = new CreatePatientResponse(request.CorrelationId());

      var toAdd = _mapper.Map<Patient>(request);
      toAdd = await _repository.AddAsync<Patient, int>(toAdd);

      var dto = _mapper.Map<PatientDto>(toAdd);
      response.Patient = dto;

      return Ok(response);
    }
  }
}
