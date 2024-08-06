using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Client;
using FastEndpoints;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.ClientEndpoints
{
  public class GetById : Endpoint<GetByIdClientRequest, Results<Ok<GetByIdClientResponse>, NotFound>>
  {
    private readonly IReadRepository<Client> _repository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Client> repository,
      IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get(GetByIdClientRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Get a Client by Id")
           .WithDescription("Gets a Client by Id")
           .WithName("clients.GetById")
           .WithTags("ClientEndpoints"));
    }

    public override async Task<Results<Ok<GetByIdClientResponse>, NotFound>> ExecuteAsync([FromRoute] GetByIdClientRequest request,
      CancellationToken cancellationToken)
    {
      var response = new GetByIdClientResponse(request.CorrelationId());

      // TODO: Use specification and consider including patients
      var client = await _repository.GetByIdAsync(request.ClientId);
      if (client is null) return TypedResults.NotFound();

      response.Client = _mapper.Map<ClientDto>(client);

      return TypedResults.Ok(response);
    }
  }
}
