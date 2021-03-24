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
  public class List : BaseAsyncEndpoint
    .WithRequest<ListPatientRequest>
    .WithResponse<ListPatientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public List(IRepository<Client> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet(ListPatientRequest.Route)]
    [SwaggerOperation(
        Summary = "List Patients",
        Description = "List Patients",
        OperationId = "patients.List",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<ListPatientResponse>> HandleAsync([FromRoute] ListPatientRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListPatientResponse(request.CorrelationId());

      var spec = new ClientByIdIncludePatientsSpecification(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return NotFound();

      response.Patients = _mapper.Map<List<PatientDto>>(client.Patients);
      response.Patients.ForEach(p =>
      {
        p.ClientName = client.FullName;
        p.ClientId = client.Id;
      });
      response.Count = response.Patients.Count;

      return Ok(response);
    }
  }
}
