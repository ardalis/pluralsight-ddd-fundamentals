using System.Collections.Generic;
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

namespace FrontDesk.Api.PatientEndpoints
{
  public class List : BaseAsyncEndpoint<ListPatientRequest, ListPatientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public List(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/patients")]
    [SwaggerOperation(
        Summary = "List Patients",
        Description = "List Patients",
        OperationId = "patients.List",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<ListPatientResponse>> HandleAsync([FromQuery] ListPatientRequest request, CancellationToken cancellationToken)
    {
      var response = new ListPatientResponse(request.CorrelationId());

      var client = await _repository.GetByIdAsync<Client, int>(request.ClientId);
      if (client == null) return NotFound();

      response.Patients = _mapper.Map<List<PatientDto>>(client.Patients);
      response.Count = response.Patients.Count;

      return Ok(response);
    }
  }
}
