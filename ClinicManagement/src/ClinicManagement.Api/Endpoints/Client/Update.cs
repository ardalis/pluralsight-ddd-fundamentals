using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Client;
using ClinicManagement.Api.ApplicationEvents;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.ClientEndpoints
{
  public class Update : BaseAsyncEndpoint
    .WithRequest<UpdateClientRequest>
    .WithResponse<UpdateClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;
    private readonly IMessagePublisher _messagePublisher;

    public Update(IRepository<Client> repository,
      IMapper mapper,
      IMessagePublisher messagePublisher)
    {
      _repository = repository;
      _mapper = mapper;
      _messagePublisher = messagePublisher;
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
      var response = new UpdateClientResponse(request.CorrelationId);

      var toUpdate = _mapper.Map<Client>(request);
      await _repository.UpdateAsync(toUpdate);

      var dto = _mapper.Map<ClientDto>(toUpdate);
      response.Client = dto;

      // Note: These messages could be triggered from the Repository or DbContext events
      // In the DbContext you could look for entities marked with an interface saying they needed
      // to be synchronized via cross-domain events and publish the appropriate message.
      var appEvent = new NamedEntityUpdatedEvent(_mapper.Map<NamedEntity>(toUpdate), "Client-Updated");
      _messagePublisher.Publish(appEvent);


      return Ok(response);
    }
  }
}
