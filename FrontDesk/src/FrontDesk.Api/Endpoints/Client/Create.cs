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
  public class Create : BaseAsyncEndpoint<CreateClientRequest, CreateClientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Create(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPost("api/clients")]
    [SwaggerOperation(
        Summary = "Creates a new Client",
        Description = "Creates a new Client",
        OperationId = "clients.create",
        Tags = new[] { "ClientEndpoints" })
    ]
    public override async Task<ActionResult<CreateClientResponse>> HandleAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
      var response = new CreateClientResponse(request.CorrelationId());

      var toAdd = _mapper.Map<Client>(request);
      toAdd = await _repository.AddAsync<Client, int>(toAdd);

      var dto = _mapper.Map<ClientDto>(toAdd);
      response.Client = dto;

      return Ok(response);
    }
  }
}
