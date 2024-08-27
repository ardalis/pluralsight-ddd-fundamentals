using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Patient;
using FastEndpoints;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.SyncedAggregates.Specifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.PatientEndpoints
{
  public class List : Endpoint<ListPatientRequest, Results<Ok<ListPatientResponse>, NotFound>>
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
      Get(ListPatientRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("List Patients")
           .WithDescription("List Patients")
           .WithName("patients.List")
           .WithTags("PatientEndpoints"));
    }

    public override async Task<Results<Ok<ListPatientResponse>, NotFound>> ExecuteAsync([FromRoute] ListPatientRequest request,
      CancellationToken cancellationToken)
    {
      var response = new ListPatientResponse(request.CorrelationId());

      var spec = new ClientByIdIncludePatientsSpecification(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return TypedResults.NotFound();

      response.Patients = _mapper.Map<List<PatientDto>>(client.Patients);
      response.Patients.ForEach(p =>
      {
        p.ClientName = client.FullName;
        p.ClientId = client.Id;
      });
      response.Count = response.Patients.Count;

      return TypedResults.Ok(response);
    }
  }
}
