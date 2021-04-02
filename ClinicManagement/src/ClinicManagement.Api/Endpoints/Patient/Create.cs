using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using ClinicManagement.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.PatientEndpoints
{
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreatePatientRequest>
    .WithResponse<CreatePatientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public Create(IRepository<Client> repository, IMapper mapper)
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
      var response = new CreatePatientResponse(request.CorrelationId);

      var spec = new ClientByIdIncludePatientsSpec(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return NotFound();

      // right now we only add huskies
      var newPatient = new Patient
      {
        ClientId = client.Id,
        Name = request.PatientName,
        AnimalType = new AnimalType("Dog", "Husky")
      };
      client.Patients.Add(newPatient);

      await _repository.UpdateAsync(client);

      var dto = _mapper.Map<PatientDto>(newPatient);
      response.Patient = dto;

      return Ok(response);
    }
  }
}
