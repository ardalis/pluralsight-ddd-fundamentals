using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using BlazorShared.Models.Schedule;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Delete : Endpoint<DeleteAppointmentRequest, Results<Ok<DeleteAppointmentResponse>, NotFound>>
  {
    private readonly IReadRepository<Schedule> _scheduleReadRepository;
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IMapper _mapper;

    public Delete(IRepository<Schedule> scheduleRepository, IReadRepository<Schedule> scheduleReadRepository, IMapper mapper)
    {
      _scheduleRepository = scheduleRepository;
      _scheduleReadRepository = scheduleReadRepository;
      _mapper = mapper;
    }

    public override void Configure()
    {
      Delete(DeleteAppointmentRequest.Route);
      AllowAnonymous();
      Description(d =>
        d.WithSummary("Deletes an Appointment")
         .WithDescription("Deletes an Appointment")
         .WithName("appointments.delete")
         .WithTags("AppointmentEndpoints"));
    }

    public override async Task<Results<Ok<DeleteAppointmentResponse>, NotFound>> ExecuteAsync(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteAppointmentResponse(request.CorrelationId());

      var spec = new ScheduleByIdWithAppointmentsSpec(request.ScheduleId); // TODO: Just get that day's appointments
      var schedule = await _scheduleReadRepository.GetBySpecAsync(spec);

      var apptToDelete = schedule.Appointments.FirstOrDefault(a => a.Id == request.AppointmentId);
      if (apptToDelete == null) return TypedResults.NotFound();

      schedule.DeleteAppointment(apptToDelete);

      await _scheduleRepository.UpdateAsync(schedule);

      // verify we can still get the schedule
      response.Schedule = _mapper.Map<ScheduleDto>(await _scheduleReadRepository.GetBySpecAsync(spec));
 
      return TypedResults.Ok(response);
    }
  }
}
