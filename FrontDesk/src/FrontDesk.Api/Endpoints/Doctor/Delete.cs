using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Doctor;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.DoctorEndpoints
{
  public class Delete : BaseAsyncEndpoint<DeleteDoctorRequest, DeleteDoctorResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete("api/doctors/{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Doctor",
        Description = "Deletes a Doctor",
        OperationId = "doctors.delete",
        Tags = new[] { "DoctorEndpoints" })
    ]
    public override async Task<ActionResult<DeleteDoctorResponse>> HandleAsync([FromRoute] DeleteDoctorRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteDoctorResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Doctor>(request);
      await _repository.DeleteAsync<Doctor, int>(toDelete);

      return Ok(response);
    }
  }
}
