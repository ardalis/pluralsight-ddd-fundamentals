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
  public class Delete : Endpoint<DeletePatientRequest, Results<Ok<DeletePatientResponse>, NotFound>>
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
      Delete("api/patients/{id}");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Deletes a Patient")
           .WithDescription("Deletes a Patient")
           .WithName("patients.delete")
           .WithTags("PatientEndpoints"));
    }

    public override async Task<Results<Ok<DeletePatientResponse>, NotFound>> ExecuteAsync(DeletePatientRequest request, CancellationToken cancellationToken)
    {
      var response = new DeletePatientResponse(request.CorrelationId);

      var spec = new ClientByIdIncludePatientsSpec(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return TypedResults.NotFound();

      var patientToDelete = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      client.Patients.Remove(patientToDelete);

      await _repository.UpdateAsync(client);

      return TypedResults.Ok(response);
    }
  }
}
