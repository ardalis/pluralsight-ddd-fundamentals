using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.PatientEndpoints
{
  public class GetById : Endpoint<GetByIdPatientRequest, Results<Ok<GetByIdPatientResponse>, NotFound>>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public GetById(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get("api/patients/{PatientId}");
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
      var response = new GetByIdPatientResponse(request.CorrelationId);

      var spec = new ClientByIdIncludePatientsSpec(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return TypedResults.NotFound();

      var patient = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      if (patient == null) return TypedResults.NotFound();

      response.Patient = _mapper.Map<PatientDto>(patient);

      return TypedResults.Ok(response);
    }
  }
}
