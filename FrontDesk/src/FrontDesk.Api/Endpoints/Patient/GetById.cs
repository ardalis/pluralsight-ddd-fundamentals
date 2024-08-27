using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Patient;
using FastEndpoints;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.SyncedAggregates.Specifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.PatientEndpoints
{
  public class GetById : Endpoint<GetByIdPatientRequest, Results<Ok<GetByIdPatientResponse>, NotFound>>
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
      Get(GetByIdPatientRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Get a Patient by Id with ClientId (via querystring)")
           .WithDescription("Gets a Patient by Id")
           .WithName("patients.GetById")
           .WithTags("PatientEndpoints"));
    }

    public override async Task<Results<Ok<GetByIdPatientResponse>, NotFound>> ExecuteAsync(GetByIdPatientRequest request, 
      CancellationToken cancellationToken)
    {
      var response = new GetByIdPatientResponse(request.CorrelationId());

      var spec = new ClientByIdIncludePatientsSpecification(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return TypedResults.NotFound();

      var patient = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      if (patient == null) return TypedResults.NotFound();

      response.Patient = _mapper.Map<PatientDto>(patient);

      return TypedResults.Ok(response);
    }
  }
}
