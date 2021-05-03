using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreateAppointmentRequest>
    .WithResponse<CreateAppointmentResponse>
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

    [HttpPost(CreateAppointmentRequest.Route)]
    [SwaggerOperation(
        Summary = "Creates a new Appointment",
        Description = "Creates a new Appointment",
        OperationId = "appointments.create",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<CreateAppointmentResponse>> HandleAsync(CreateAppointmentRequest request,
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

      return Ok(response);
    }
  }
}
