using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Client;
using ClinicManagement.Contracts;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Interfaces;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.ClientEndpoints
{
  public class Update : Endpoint<UpdateClientRequest, UpdateClientResponse>
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

    public override void Configure()
    {
      Put("api/clients");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Updates a Client")
           .WithDescription("Updates a Client")
           .WithName("clients.update")
           .WithTags("ClientEndpoints"));
    }

    public override async Task<UpdateClientResponse> ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateClientResponse(request.CorrelationId);

      var toUpdate = _mapper.Map<Client>(request);
      await _repository.UpdateAsync(toUpdate);

      var dto = _mapper.Map<ClientDto>(toUpdate);
      response.Client = dto;

      // Note: These messages could be triggered from the Repository or DbContext events
      // In the DbContext you could look for entities marked with an interface saying they needed
      // to be synchronized via cross-domain events and publish the appropriate message.
      var appEvent = new ClientUpdatedIntegrationEvent(toUpdate.Id, toUpdate.FullName);
      await _messagePublisher.Publish(appEvent);

      return response;
    }
  }
}
