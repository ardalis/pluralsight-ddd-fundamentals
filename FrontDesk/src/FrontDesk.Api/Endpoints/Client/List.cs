using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Client;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.ClientEndpoints
{
  public class List : BaseAsyncEndpoint<ListClientRequest, ListClientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public List(IRepository repository, IMapper mapper)
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
      var response = new ListClientResponse(request.CorrelationId());

      var clients = await _repository.ListAsync<Client, int>();
      if (clients is null) return NotFound();

      response.Clients = _mapper.Map<List<ClientDto>>(clients);
      response.Count = response.Clients.Count;

      return Ok(response);
    }
  }
}