using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.PatientEndpoints
{
  public class GetById : BaseAsyncEndpoint
    .WithRequest<GetByIdPatientRequest>
    .WithResponse<GetByIdPatientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/patients/{PatientId}")]
    [SwaggerOperation(
        Summary = "Get a Patient by Id with ClientId (via querystring)",
        Description = "Gets a Patient by Id",
        OperationId = "patients.GetById",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdPatientResponse>> HandleAsync([FromRoute] GetByIdPatientRequest request, 
      CancellationToken cancellationToken)
    {
      var response = new GetByIdPatientResponse(request.CorrelationId);

      var spec = new ClientByIdIncludePatientsSpec(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return NotFound();

      var patient = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      if (patient == null) return NotFound();

      response.Patient = _mapper.Map<PatientDto>(patient);

      return Ok(response);
    }
  }
}
