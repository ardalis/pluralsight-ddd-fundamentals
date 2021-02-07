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
  public class Delete : BaseAsyncEndpoint<DeleteClientRequest, DeleteClientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete("api/clients/{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Client",
        Description = "Deletes a Client",
        OperationId = "clients.delete",
        Tags = new[] { "ClientEndpoints" })
    ]
    public override async Task<ActionResult<DeleteClientResponse>> HandleAsync([FromRoute] DeleteClientRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteClientResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Client>(request);
      await _repository.DeleteAsync<Client, int>(toDelete);

      return Ok(response);
    }
  }
}
