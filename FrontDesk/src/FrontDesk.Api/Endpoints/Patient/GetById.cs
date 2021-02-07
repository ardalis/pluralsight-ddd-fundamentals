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
  public class GetById : BaseAsyncEndpoint<GetByIdPatientRequest, GetByIdPatientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/patients/{PatientId}")]
    [SwaggerOperation(
        Summary = "Get a Patient by Id",
        Description = "Gets a Patient by Id",
        OperationId = "patients.GetById",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdPatientResponse>> HandleAsync([FromRoute] GetByIdPatientRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdPatientResponse(request.CorrelationId());

      var patient = await _repository.GetByIdAsync<Patient, int>(request.PatientId);
      if (patient is null) return NotFound();

      response.Patient = _mapper.Map<PatientDto>(patient);

      return Ok(response);
    }
  }


}
