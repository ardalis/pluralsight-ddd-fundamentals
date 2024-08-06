using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.AppointmentType;
using ClinicManagement.Core.Aggregates;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace ClinicManagement.Api.AppointmentTypeEndpoints
{
  public class List : Endpoint<ListAppointmentTypeRequest, ListAppointmentTypeResponse>
  {
    private readonly IRepository<AppointmentType> _repository;
    private readonly IMapper _mapper;

    public List(IRepository<AppointmentType> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get("api/appointment-types");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("List Appointment Types")
           .WithDescription("List Appointment Types")
           .WithName("appointment-types.List")
           .WithTags("AppointmentTypeEndpoints"));
    }

    public override async Task<ListAppointmentTypeResponse> ExecuteAsync(ListAppointmentTypeRequest request, CancellationToken cancellationToken)
    {
      var response = new ListAppointmentTypeResponse(request.CorrelationId);

      var appointmentTypes = await _repository.ListAsync();
      response.AppointmentTypes = _mapper.Map<List<AppointmentTypeDto>>(appointmentTypes);
      response.Count = response.AppointmentTypes.Count;

      return response;
    }
  }
}
