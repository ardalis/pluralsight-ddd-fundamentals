using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.AppointmentType;
using FastEndpoints;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.AppointmentTypeEndpoints
{
  public class List : Endpoint<ListAppointmentTypeRequest, ListAppointmentTypeResponse>
  {
    private readonly IReadRepository<AppointmentType> _appointmentTypeRepository;
    private readonly IMapper _mapper;

    public List(IReadRepository<AppointmentType> appointmentTypeRepository, IMapper mapper)
    {
      _appointmentTypeRepository = appointmentTypeRepository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get(ListAppointmentTypeRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("List Appointment Types")
           .WithDescription("List Appointment Types")
           .WithName("appointment-types.List")
           .WithTags("AppointmentTypeEndpoints"));
    }

    public override async Task<ListAppointmentTypeResponse> ExecuteAsync(ListAppointmentTypeRequest request, CancellationToken cancellationToken)
    {
      var response = new ListAppointmentTypeResponse(request.CorrelationId());

      var appointmentTypes = await _appointmentTypeRepository.ListAsync();
      response.AppointmentTypes = _mapper.Map<List<AppointmentTypeDto>>(appointmentTypes);
      response.Count = response.AppointmentTypes.Count;

      return response;
    }
  }
}
