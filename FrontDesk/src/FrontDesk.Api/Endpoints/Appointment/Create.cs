using System;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Create : Endpoint<CreateAppointmentRequest, CreateAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IReadRepository<AppointmentType> _appointmentTypeReadRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Create> _logger;

    public Create(IRepository<Schedule> scheduleRepository,
      IReadRepository<AppointmentType> appointmentTypeReadRepository,
      IMapper mapper,
      ILogger<Create> logger)
    {
      _scheduleRepository = scheduleRepository;
      _appointmentTypeReadRepository = appointmentTypeReadRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public override void Configure()
    {
      Post(CreateAppointmentRequest.Route);
      AllowAnonymous();
      Description(d =>
        d.WithSummary("Creates a new Appointment")
         .WithDescription("Creates a new Appointment")
         .WithName("appointments.create")
         .WithTags("AppointmentEndpoints"));
    }

    public override async Task<CreateAppointmentResponse> ExecuteAsync(CreateAppointmentRequest request,
      CancellationToken cancellationToken)
    {
      var response = new CreateAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      var appointmentType = await _appointmentTypeReadRepository.GetByIdAsync(request.AppointmentTypeId);
      var appointmentStart = request.DateOfAppointment;
      var timeRange = new DateTimeOffsetRange(appointmentStart, TimeSpan.FromMinutes(appointmentType.Duration));

      var newAppointment = new Appointment(Guid.NewGuid(), request.AppointmentTypeId, request.ScheduleId,
        request.ClientId, request.SelectedDoctor, request.PatientId, request.RoomId, timeRange, request.Title);

      schedule.AddNewAppointment(newAppointment);

      await _scheduleRepository.UpdateAsync(schedule);
      _logger.LogInformation($"Appointment created for patient {request.PatientId} with Id {newAppointment.Id}");

      var dto = _mapper.Map<AppointmentDto>(newAppointment);
      _logger.LogInformation(dto.ToString());
      response.Appointment = dto;

      return response;
    }
  }
}
