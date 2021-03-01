using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Appointment;
using FrontDesk.Core.Aggregates;
using FrontDesk.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Update : BaseAsyncEndpoint
    .WithRequest<UpdateAppointmentRequest>
    .WithResponse<UpdateAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IMapper _mapper;

    public Update(IRepository<Schedule> scheduleRepository, IMapper mapper)
    {
      _scheduleRepository = scheduleRepository;
      _mapper = mapper;
    }

    [HttpPut("api/appointments")]
    [SwaggerOperation(
        Summary = "Updates a Appointment",
        Description = "Updates a Appointment",
        OperationId = "appointments.update",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<UpdateAppointmentResponse>> HandleAsync(UpdateAppointmentRequest request,
      CancellationToken cancellationToken)
    {
      var response = new UpdateAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      var apptToUpdate = schedule.Appointments.FirstOrDefault(a => a.Id == request.Id);
      apptToUpdate.UpdateRoom(request.RoomId);
      apptToUpdate.UpdateTime(new PluralsightDdd.SharedKernel.DateTimeRange(request.Start, request.End));

      // TODO: Implement updating other properties

      await _scheduleRepository.UpdateAsync(schedule);

      var dto = _mapper.Map<AppointmentDto>(apptToUpdate);
      response.Appointment = dto;

      return Ok(response);
    }
  }
}
