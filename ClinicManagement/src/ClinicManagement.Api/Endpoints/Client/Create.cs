using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Client;
using ClinicManagement.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.ClientEndpoints
{
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreateClientRequest>
    .WithResponse<CreateClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public Create(IRepository<Client> repository, IMapper mapper)
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
      var response = new CreateClientResponse(request.CorrelationId);

      var toAdd = _mapper.Map<Client>(request);
      toAdd = await _repository.AddAsync(toAdd);

      var dto = _mapper.Map<ClientDto>(toAdd);
      response.Client = dto;

      return Ok(response);
    }
  }
}
