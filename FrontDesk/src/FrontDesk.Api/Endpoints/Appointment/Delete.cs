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
  public class Delete : BaseAsyncEndpoint
    .WithRequest<DeleteAppointmentRequest>
    .WithResponse<DeleteAppointmentResponse>
  {
    private readonly IRepository<Schedule> _scheduleRepository;

    public Delete(IRepository<Schedule> scheduleRepository)
    {
      _scheduleRepository = scheduleRepository;
    }

    [HttpDelete(DeleteAppointmentRequest.Route)]
    [SwaggerOperation(
        Summary = "Deletes a Appointment",
        Description = "Deletes a Appointment",
        OperationId = "appointments.delete",
        Tags = new[] { "AppointmentEndpoints" })
    ]
    public override async Task<ActionResult<DeleteAppointmentResponse>> HandleAsync([FromRoute] DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleRepository.GetBySpecAsync(spec);

      var apptToDelete = schedule.Appointments.FirstOrDefault(a => a.Id == request.AppointmentId);
      if (apptToDelete == null) return NotFound();

      schedule.DeleteAppointment(apptToDelete);

      await _scheduleRepository.UpdateAsync(schedule);

      // verify we can still get the schedule
      var s = await _scheduleRepository.GetBySpecAsync(spec);

      return Ok(response);
    }
  }
}
