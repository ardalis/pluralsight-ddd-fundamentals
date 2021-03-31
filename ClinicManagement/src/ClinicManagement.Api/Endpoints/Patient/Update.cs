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
  public class Update : BaseAsyncEndpoint
    .WithRequest<UpdatePatientRequest>
    .WithResponse<UpdatePatientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public Update(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPut("api/patients")]
    [SwaggerOperation(
        Summary = "Updates a Patient",
        Description = "Updates a Patient",
        OperationId = "patients.update",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<UpdatePatientResponse>> HandleAsync(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdatePatientResponse(request.CorrelationId);

      var spec = new ClientByIdIncludePatientsSpec(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return NotFound();

      var patientToUpdate = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      if (patientToUpdate == null) return NotFound();

      patientToUpdate.Name = request.Name;

      await _repository.UpdateAsync(client);

      var dto = _mapper.Map<PatientDto>(patientToUpdate);
      response.Patient = dto;

      return Ok(response);
    }
  }
}
