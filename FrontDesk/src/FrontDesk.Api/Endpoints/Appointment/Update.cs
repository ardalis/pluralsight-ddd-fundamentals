using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using FastEndpoints;
using FrontDesk.Core.ScheduleAggregate;
using FrontDesk.Core.ScheduleAggregate.Specifications;
using FrontDesk.Core.SyncedAggregates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PluralsightDdd.SharedKernel.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace FrontDesk.Api.AppointmentEndpoints
{
  public class Update : Endpoint<UpdateAppointmentRequest, UpdateAppointmentResponse>
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

    public override void Configure()
    {
      Put(UpdateAppointmentRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Updates an Appointment")
           .WithDescription("Updates an Appointment")
           .WithName("appointments.update")
           .WithTags("AppointmentEndpoints"));
    }

    public override async Task<UpdateAppointmentResponse> ExecuteAsync(UpdateAppointmentRequest request,
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

      return response;
    }
  }
}
