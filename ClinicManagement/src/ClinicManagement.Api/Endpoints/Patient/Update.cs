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
  public class Update : Endpoint<UpdatePatientRequest, Results<Ok<UpdatePatientResponse>, NotFound>>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;

    public Update(IRepository<Client> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Put("api/patients");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Updates a Patient")
           .WithDescription("Updates a Patient")
           .WithName("patients.update")
           .WithTags("PatientEndpoints"));
    }

    public override async Task<Results<Ok<UpdatePatientResponse>, NotFound>> ExecuteAsync(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdatePatientResponse(request.CorrelationId);

      var spec = new ClientByIdIncludePatientsSpec(request.ClientId);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return TypedResults.NotFound();

      var patientToUpdate = client.Patients.FirstOrDefault(p => p.Id == request.PatientId);
      if (patientToUpdate == null) return TypedResults.NotFound();

      patientToUpdate.Name = request.Name;

      await _repository.UpdateAsync(client);

      var dto = _mapper.Map<PatientDto>(patientToUpdate);
      response.Patient = dto;

      return TypedResults.Ok(response);
    }
  }
}
