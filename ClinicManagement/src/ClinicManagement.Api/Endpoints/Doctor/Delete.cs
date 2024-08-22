using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Doctor;
using ClinicManagement.Core.Aggregates;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.DoctorEndpoints
{
  public class Delete : Endpoint<DeleteDoctorRequest, DeleteDoctorResponse>
  {
    private readonly IRepository<Doctor> _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository<Doctor> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Delete("api/doctors/{id}");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Deletes a Doctor")
           .WithDescription("Deletes a Doctor")
           .WithName("doctors.delete")
           .WithTags("DoctorEndpoints"));
    }

    public override async Task<DeleteDoctorResponse> ExecuteAsync(DeleteDoctorRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteDoctorResponse(request.CorrelationId);

      var toDelete = new Doctor(request.Id, "to delete");
      await _repository.DeleteAsync(toDelete);

      return response;
    }
  }
}
