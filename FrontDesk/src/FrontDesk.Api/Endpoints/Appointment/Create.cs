using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreateAppointmentRequest>
    .WithResponse<CreateAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IRepository<AppointmentType> _appointmentTypeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Create> _logger;

    public Create(IRepository<Schedule> scheduleRepository,
      IRepository<AppointmentType> appointmentTypeRepository,
      IMapper mapper,
      ILogger<Create> logger)
    {
      _scheduleRepository = scheduleRepository;
      _appointmentTypeRepository = appointmentTypeRepository;
      _mapper = mapper;
      _logger = logger;
    }

    [HttpPost("api/appointments")]
    [SwaggerOperation(
        Summary = "Creates a new Appointment",
        Description = "Creates a new Appointment",
        OperationId = "appointments.create",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<CreateAppointmentResponse>> HandleAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new CreateAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      var appointmentType = await _appointmentTypeRepository.GetByIdAsync(request.AppointmentTypeId);

      var newAppointment = Appointment.Create(request.ScheduleId, request.ClientId, request.PatientId, request.RoomId, request.DateOfAppointment, request.DateOfAppointment.AddMinutes(appointmentType.Duration), request.AppointmentTypeId, request.SelectedDoctor, request.Details);

      schedule.AddNewAppointment(newAppointment);

      _logger.LogDebug($"Adding appointment to schedule. Total appointments: {schedule.Appointments.Count()}");

      await _scheduleRepository.UpdateAsync(schedule);

      _logger.LogInformation($"Appointment created for patient {request.PatientId} with Id {newAppointment.Id}");

      var dto = _mapper.Map<AppointmentDto>(newAppointment);
      _logger.LogInformation(dto.ToString());
      response.Appointment = dto;

      return Ok(response);
    }
  }
}
