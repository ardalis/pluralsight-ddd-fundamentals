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
  public class Delete : BaseAsyncEndpoint<DeletePatientRequest, DeletePatientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete("api/patients/{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Patient",
        Description = "Deletes a Patient",
        OperationId = "patients.delete",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<DeletePatientResponse>> HandleAsync([FromRoute] DeletePatientRequest request, CancellationToken cancellationToken)
    {
      var response = new DeletePatientResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Patient>(request);
      await _repository.DeleteAsync<Patient, int>(toDelete);

      return Ok(response);
    }
  }
}
