using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Client;
using FastEndpoints;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.SyncedAggregates.Specifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.ClientEndpoints
{
  public class List : Endpoint<ListClientRequest, Results<Ok<ListClientResponse>, NotFound>>
  {
    private readonly IReadRepository<Client> _repository;
    private readonly IMapper _mapper;

    public List(IReadRepository<Client> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get("api/clients");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("List Clients")
           .WithDescription("List Clients")
           .WithName("clients.List")
           .WithTags("ClientEndpoints"));
    }

    public override async Task<Results<Ok<ListClientResponse>, NotFound>> ExecuteAsync(ListClientRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListClientResponse(request.CorrelationId());

      var spec = new ClientsIncludePatientsSpecification();
      var clients = await _repository.ListAsync(spec);
      if (clients is null) return TypedResults.NotFound();

      response.Clients = _mapper.Map<List<ClientDto>>(clients);
      response.Count = response.Clients.Count;

      return TypedResults.Ok(response);
    }
  }
}
