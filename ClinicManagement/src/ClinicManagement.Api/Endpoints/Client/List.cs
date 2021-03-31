using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Client;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.ClientEndpoints
{
  public class List : BaseAsyncEndpoint
    .WithRequest<ListClientRequest>
    .WithResponse<ListClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public List(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/clients")]
    [SwaggerOperation(
        Summary = "List Clients",
        Description = "List Clients",
        OperationId = "clients.List",
        Tags = new[] { "ClientEndpoints" })
    ]
    public override async Task<ActionResult<ListClientResponse>> HandleAsync([FromQuery] ListClientRequest request, CancellationToken cancellationToken)
    {
      var response = new ListClientResponse(request.CorrelationId);

      var spec = new ClientsIncludePatientsSpec();
      var clients = await _repository.ListAsync(spec);
      if (clients is null) return NotFound();

      response.Clients = _mapper.Map<List<ClientDto>>(clients);
      response.Count = response.Clients.Count;

      return Ok(response);
    }
  }
}
