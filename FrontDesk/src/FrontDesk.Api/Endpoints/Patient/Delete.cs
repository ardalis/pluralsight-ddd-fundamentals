using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Patient;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace FrontDesk.Api.PatientEndpoints
{
  public class Delete : BaseAsyncEndpoint
    .WithRequest<DeletePatientRequest>
    .WithResponse<DeletePatientResponse>
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

      var spec = new ClientByIdIncludePatientsSpecification(request.ClientId);
      var client = await _repository.GetAsync<Client, int>(spec);
      if (client == null) return NotFound();

      var patientToDelete = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      client.Patients.Remove(patientToDelete);

      await _repository.UpdateAsync<Client, int>(client);

      return Ok(response);
    }
  }
}
