using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.SyncedAggregates;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Update : BaseAsyncEndpoint
    .WithRequest<UpdateAppointmentRequest>
    .WithResponse<UpdateAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IReadRepository<Schedule> _scheduleReadRepository;
    private readonly IReadRepository<AppointmentType> _appointmentTypeRepository;
    private readonly IMapper _mapper;

    public Update(IRepository<Schedule> scheduleRepository,
      IReadRepository<Schedule> scheduleReadRepository,
      IReadRepository<AppointmentType> appointmentTypeRepository,
      IMapper mapper)
    {
      _scheduleRepository = scheduleRepository;
      _scheduleReadRepository = scheduleReadRepository;
      _appointmentTypeRepository = appointmentTypeRepository;
      _mapper = mapper;
    }

    [HttpPut(UpdateAppointmentRequest.Route)]
    [SwaggerOperation(
        Summary = "Updates an Appointment",
        Description = "Updates an Appointment",
        OperationId = "appointments.update",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<UpdateAppointmentResponse>> HandleAsync(UpdateAppointmentRequest request,
      CancellationToken cancellationToken)
    {
      var response = new UpdateAppointmentResponse(request.CorrelationId());

      var apptType = await _appointmentTypeRepository.GetByIdAsync(request.AppointmentTypeId);

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleReadRepository.GetBySpecAsync(spec);

      var apptToUpdate = schedule.Appointments
                            .FirstOrDefault(a => a.Id == request.Id);
      apptToUpdate.UpdateAppointmentType(apptType, schedule.AppointmentUpdatedHandler);
      apptToUpdate.UpdateRoom(request.RoomId);
      apptToUpdate.UpdateStartTime(request.Start, schedule.AppointmentUpdatedHandler);
      apptToUpdate.UpdateTitle(request.Title);
      apptToUpdate.UpdateDoctor(request.DoctorId);

      await _scheduleRepository.UpdateAsync(schedule);

      var dto = _mapper.Map<AppointmentDto>(apptToUpdate);
      response.Appointment = dto;

      return Ok(response);
    }
  }
}
