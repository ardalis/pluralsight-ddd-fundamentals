using System.Linq;
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
  public class Update : BaseAsyncEndpoint<UpdatePatientRequest, UpdatePatientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Update(IRepository repository, IMapper mapper)
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
      var response = new UpdatePatientResponse(request.CorrelationId());

      var client = await _repository.GetByIdAsync<Client, int>(request.ClientId);
      if (client == null) return NotFound();

      var patientToUpdate = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      if (patientToUpdate == null) return NotFound();

      patientToUpdate.UpdateName(request.Name);

      await _repository.UpdateAsync<Client, int>(client);

      var dto = _mapper.Map<PatientDto>(patientToUpdate);
      response.Patient = dto;

      return Ok(response);
    }
  }
}
