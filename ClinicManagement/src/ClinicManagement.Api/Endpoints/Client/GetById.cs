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
  public class GetById : BaseAsyncEndpoint
    .WithRequest<GetByIdClientRequest>
    .WithResponse<GetByIdClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/clients/{ClientId}")]
    [SwaggerOperation(
        Summary = "Get a Client by Id",
        Description = "Gets a Client by Id",
        OperationId = "clients.GetById",
        Tags = new[] { "ClientEndpoints" })
    ]
    public override async Task<ActionResult<GetByIdClientResponse>> HandleAsync([FromRoute] GetByIdClientRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdClientResponse(request.CorrelationId);

      var client = await _repository.GetByIdAsync(request.ClientId);
      if (client is null) return NotFound();

      response.Client = _mapper.Map<ClientDto>(client);

      return Ok(response);
    }
  }


}
