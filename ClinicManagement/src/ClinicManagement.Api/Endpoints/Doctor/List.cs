using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Doctor;
using ClinicManagement.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.DoctorEndpoints
{
  public class List : BaseAsyncEndpoint
    .WithRequest<ListDoctorRequest>
    .WithResponse<ListDoctorResponse>
  {
    private readonly IRepository<Doctor> _repository;
    private readonly IMapper _mapper;

    public List(IRepository<Doctor> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet("api/doctors")]
    [SwaggerOperation(
        Summary = "List Doctors",
        Description = "List Doctors",
        OperationId = "doctors.List",
        Tags = new[] { "DoctorEndpoints" })
    ]
    public override async Task<ActionResult<ListDoctorResponse>> HandleAsync([FromQuery] ListDoctorRequest request, CancellationToken cancellationToken)
    {
      var response = new ListDoctorResponse(request.CorrelationId);

      var doctors = await _repository.ListAsync();
      if (doctors is null) return NotFound();

      response.Doctors = _mapper.Map<List<DoctorDto>>(doctors);
      response.Count = response.Doctors.Count;

      return Ok(response);
    }
  }
}
