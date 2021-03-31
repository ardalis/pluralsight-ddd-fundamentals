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
  public class Delete : BaseAsyncEndpoint
    .WithRequest<DeleteClientRequest>
    .WithResponse<DeleteClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository<Client> repository, IMapper mapper)
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
      var response = new DeleteClientResponse(request.CorrelationId);

      var toDelete = _mapper.Map<Client>(request);
      await _repository.DeleteAsync(toDelete);

      return Ok(response);
    }
  }
}
