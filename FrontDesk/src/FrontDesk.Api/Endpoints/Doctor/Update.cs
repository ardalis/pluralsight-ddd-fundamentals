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
  public class Update : BaseAsyncEndpoint<UpdateDoctorRequest, UpdateDoctorResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Update(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPut("api/doctors")]
    [SwaggerOperation(
        Summary = "Updates a Doctor",
        Description = "Updates a Doctor",
        OperationId = "doctors.update",
        Tags = new[] { "DoctorEndpoints" })
    ]
    public override async Task<ActionResult<UpdateDoctorResponse>> HandleAsync(UpdateDoctorRequest request, CancellationToken cancellationToken)
    {
      var response = new UpdateDoctorResponse(request.CorrelationId());

      var toUpdate = _mapper.Map<Doctor>(request);
      await _repository.UpdateAsync<Doctor, int>(toUpdate);

      var dto = _mapper.Map<DoctorDto>(toUpdate);
      response.Doctor = dto;

      return Ok(response);
    }
  }
}
