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
  public class Update : BaseAsyncEndpoint<UpdateClientRequest, UpdateClientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Update(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPut("api/clients")]
    [SwaggerOperation(
        Summary = "Updates a Client",
        Description = "Updates a Client",
        OperationId = "clients.update",
        Tags = new[] { "ClientEndpoints" })
    ]
    public override async Task<ActionResult<UpdateClientResponse>> HandleAsync(UpdateClientRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateClientResponse(request.CorrelationId());

      var toUpdate = _mapper.Map<Client>(request);
      await _repository.UpdateAsync<Client, int>(toUpdate);

      var dto = _mapper.Map<ClientDto>(toUpdate);
      response.Client = dto;

      return Ok(response);
    }
  }
}
