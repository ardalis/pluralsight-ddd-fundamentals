using System;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<Create> _logger;

    public Create(IRepository repository, IMapper mapper, ILogger<Create> logger)
    {
      _repository = repository;
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

      var appointmentType = await _repository.GetByIdAsync<AppointmentType, int>(request.AppointmentTypeId);

      var newAppointment = Appointment.Create(request.ScheduleId, request.ClientId, request.PatientId, request.RoomId, request.DateOfAppointment, request.DateOfAppointment.AddMinutes(appointmentType.Duration), request.AppointmentTypeId, request.SelectedDoctor, request.Details);

      newAppointment = await _repository.AddAsync<Appointment, Guid>(newAppointment);
      _logger.LogInformation($"Appointment created for patient {request.PatientId} with Id {newAppointment.Id}");

      var dto = _mapper.Map<AppointmentDto>(newAppointment);
      _logger.LogInformation(dto.ToString());
      response.Appointment = dto;

      return Ok(response);
    }
  }
}
