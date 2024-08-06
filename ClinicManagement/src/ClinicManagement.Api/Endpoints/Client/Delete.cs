using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Client;
using ClinicManagement.Core.Aggregates;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.ClientEndpoints
{
  public class Delete : Endpoint<DeleteClientRequest, DeleteClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Delete("api/clients/{id}");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Deletes a Client")
           .WithDescription("Deletes a Client")
           .WithName("clients.delete")
           .WithTags("ClientEndpoints"));
    }

    public override async Task<DeleteClientResponse> ExecuteAsync(DeleteClientRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteClientResponse(request.CorrelationId);

      var toDelete = _mapper.Map<Client>(request);
      await _repository.DeleteAsync(toDelete);

      return response;
    }
  }
}
