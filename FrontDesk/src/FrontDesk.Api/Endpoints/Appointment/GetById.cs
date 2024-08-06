using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.SyncedAggregates.Specifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class GetById : Endpoint<GetByIdAppointmentRequest, Results<Ok<GetByIdAppointmentResponse>, NotFound>>
  {
    private readonly IReadRepository<Schedule> _scheduleRepository;
    private readonly IReadRepository<Client> _clientRepository;
    private readonly IMapper _mapper;

    public GetById(IReadRepository<Schedule> scheduleRepository,
      IReadRepository<Client> clientRepository,
      IMapper mapper)
    {
      _scheduleRepository = scheduleRepository;
      _clientRepository = clientRepository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Get(GetByIdAppointmentRequest.Route);
      AllowAnonymous();
      Description(d =>
        d.WithSummary("Get an Appointment by Id")
         .WithDescription("Gets an Appointment by Id")
         .WithName("appointments.GetById")
         .WithTags("AppointmentEndpoints"));
    }

    public override async Task<Results<Ok<GetByIdAppointmentResponse>, NotFound>> ExecuteAsync(GetByIdAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new GetByIdAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      var appointment = schedule.Appointments.FirstOrDefault(a => a.Id == request.AppointmentId);
      if (appointment == null) return TypedResults.NotFound();

      response.Appointment = _mapper.Map<AppointmentDto>(appointment);

      // load names
      var clientSpec = new ClientByIdIncludePatientsSpecification(appointment.ClientId);
      var client = await _clientRepository.GetBySpecAsync(clientSpec);
      var patient = client.Patients.First(p => p.Id == appointment.PatientId);

      response.Appointment.ClientName = client.FullName;
      response.Appointment.PatientName = patient.Name;

      return TypedResults.Ok(response);
    }
  }
}
