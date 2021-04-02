using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using BlazorShared.Models.AppointmentType;
using BlazorShared.Models.Doctor;
using BlazorShared.Models.Patient;
using FrontDesk.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FrontDesk.Blazor.Pages
{
  public partial class AppointmentEditor
  {
    [Inject]
    ILogger<AppointmentEditor> Logger { get; set; }

    [Inject]
    AppointmentTypeService AppointmentTypeService { get; set; }

    [Inject]
    DoctorService DoctorService { get; set; }

    [Inject]
    AppointmentService AppointmentService { get; set; }

    [Inject]
    FileService FileService { get; set; }

    //communication with the parent component where the scheduler is
    [Parameter] public AppointmentDto Appointment { get; set; } = new AppointmentDto();
    [Parameter] public PatientDto Patient { get; set; }
    [Parameter] public Guid ScheduleId { get; set; } = Guid.Empty;
    [Parameter] public int RoomId { get; set; } = 0;
    [Parameter] public EventCallback<string> OnAppointmentChanged { get; set; }

    List<DoctorDto> Doctors = new List<DoctorDto>();
    List<AppointmentTypeDto> AppointmentTypes = new List<AppointmentTypeDto>();

    //UI related
    string WrongRangeMessage = "Start date must be before end date. We reset the selection, try again.";
    bool ShowErrorMessage { get; set; }
    string GetDuration => AppointmentTypes.FirstOrDefault(x => x.AppointmentTypeId == Appointment.AppointmentTypeId)?.Duration.ToString();
    private string Picture { get; set; }
    private bool IsAdd => Appointment == null || Appointment.AppointmentId == Guid.Empty;
    private string Title
    {
      get
      {
        if (IsAdd)
        {
          var appointmentType = AppointmentTypes.FirstOrDefault(a => a.AppointmentTypeId == Appointment.AppointmentTypeId);
          if (appointmentType == null)
          {
            return $"{Patient?.Name} - {Patient?.ClientName}";
          }
          Appointment.Title = $"({appointmentType.Code}) {Patient?.Name} - {Patient?.ClientName}";

          return Appointment.Title;
        }
        else
        {
          var appointmentType = AppointmentTypes.FirstOrDefault(a => a.AppointmentTypeId == Appointment.AppointmentTypeId);
          if (appointmentType == null)
          {
            return $"{Appointment.PatientName} - {Appointment?.ClientName}";
          }
          Appointment.Title = $"({appointmentType.Code}) {Appointment.PatientName} - {Appointment?.ClientName}";

          return Appointment.Title;
        }
      }
    }

    protected override async Task OnInitializedAsync()
    {
      Logger.LogInformation($"Initialize AppointmentEditor with appointment starting at {Appointment.Start}");
      Doctors = await DoctorService.ListAsync();
      if (IsAdd)
      {
        Appointment.DoctorId = Doctors[0].DoctorId;
      }

      AppointmentTypes = await AppointmentTypeService.ListAsync();
      if (IsAdd)
      {
        Appointment.AppointmentTypeId = AppointmentTypes[0].AppointmentTypeId;
      }

      // set appointment to current patient if not specified
      if(string.IsNullOrEmpty(Appointment.PatientName))
      {
        Appointment.PatientName = Patient.Name;
        Appointment.PatientId = Patient.PatientId;
        Appointment.ClientId = Patient.ClientId;
        Appointment.ClientName = Patient.ClientName;
        Appointment.Title = $"(WE) {Appointment.PatientName} - {Appointment.ClientName}";
        Logger.LogInformation("Setting new appointment up for " + Appointment.Title);
      }

      await LoadPicture();
    }

    private async Task LoadPicture()
    {
      var imageData = await FileService.ReadPicture($"{Appointment.PatientName}.jpg");
      Picture = string.IsNullOrEmpty(imageData) ? string.Empty : $"data:image/png;base64, {imageData}";
    }

    private bool IsValid()
    {
      //re-implements the model validation to ensure the extra functionality in the form works
      //there are ways to extend the validators to also support range date validation but they
      //are beyond the scope of this example, and here we will do things in a simpler, more straightforward way
      //this is why range validation is also implemented with code here, not as validation attributes
      if (Appointment.Start <= Appointment.End)
      {
        return true;
      }
      return false;
    }

    private async Task SaveAppointment()
    {
      if (IsValid())
      {
        if (Appointment.AppointmentId == Guid.Empty)
        {
          CreateAppointmentRequest toCreate = new CreateAppointmentRequest()
          {
            Title = Title,
            SelectedDoctor = (int)Appointment.DoctorId,
            PatientId = Patient.PatientId,
            ClientId = Patient.ClientId,
            ScheduleId = ScheduleId,
            RoomId = Appointment.RoomId,
            AppointmentTypeId = Appointment.AppointmentTypeId,
            DateOfAppointment = Appointment.Start.DateTime,
          };
          Logger.LogInformation($"Creating appointment starting at {toCreate.DateOfAppointment}");

          await AppointmentService.CreateAsync(toCreate);
        }
        else
        {
          var toUpdate = UpdateAppointmentRequest.FromDto(Appointment);
          await AppointmentService.EditAsync(toUpdate);
        }

        await OnAppointmentChanged.InvokeAsync(Appointment.Title);
      }
    }

    private async Task DeleteAppointment()
    {
      await AppointmentService.DeleteAsync(Appointment.ScheduleId, Appointment.AppointmentId);
      await OnAppointmentChanged.InvokeAsync(Appointment.Title);
    }

    private Task CancelEditing()
    {
      return OnAppointmentChanged.InvokeAsync(Appointment.Title);
    }

    private void AppointmentTypeUpdated(ChangeEventArgs e)
    {
      Appointment.AppointmentTypeId = (int?)e.Value ?? 0;
    }
  }
}
