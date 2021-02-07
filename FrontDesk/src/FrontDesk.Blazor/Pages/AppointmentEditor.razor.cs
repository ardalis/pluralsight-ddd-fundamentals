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

namespace FrontDesk.Blazor.Pages
{
  public partial class AppointmentEditor
  {
    [Inject]
    AppointmentTypeService AppointmentTypeService { get; set; }

    [Inject]
    DoctorService DoctorService { get; set; }

    [Inject]
    AppointmentService AppointmentService { get; set; }

    [Inject]
    FileService FileService { get; set; }

    //communcation with the parent component where the scheduler is
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

      await LoadPicture();
    }

    private async Task LoadPicture()
    {
      if (string.IsNullOrEmpty(Appointment.PatientName))
      {
        return;
      }

      var imgeData = await FileService.ReadPicture($"{Appointment.PatientName}.jpg");
      Picture = string.IsNullOrEmpty(imgeData) ? string.Empty : $"data:image/png;base64, {imgeData}";
    }

    private bool IsValid()
    {
      //re-implements the model validation to ensure the exra functionality in the form works
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
            Details = Title,
            SelectedDoctor = (int)Appointment.DoctorId,
            PatientId = Patient.PatientId,
            ClientId = Patient.ClientId,
            ScheduleId = ScheduleId,
            RoomId = Appointment.RoomId,
            AppointmentTypeId = Appointment.AppointmentTypeId,
            DateOfAppointment = Appointment.Start,
          };

          await AppointmentService.CreateAsync(toCreate);
        }
        else
        {
          UpdateAppointmentRequest toUpdate = new UpdateAppointmentRequest()
          {
            Id = Appointment.AppointmentId,
            DoctorId = (int)Appointment.DoctorId,
            PatientId = Patient.PatientId,
            Title = Appointment.Title,
            ClientId = Patient.ClientId,
            ScheduleId = ScheduleId,
            RoomId = Appointment.RoomId,
            AppointmentTypeId = Appointment.AppointmentTypeId,
            Start = Appointment.Start,
            End = Appointment.End,
          };
          await AppointmentService.EditAsync(toUpdate);
        }

        await OnAppointmentChanged.InvokeAsync(Appointment.Title);
      }
    }

    private async Task DeleteAppointment()
    {
      await AppointmentService.DeleteAsync(Appointment.AppointmentId);
      await OnAppointmentChanged.InvokeAsync(Appointment.Title);
    }

    private Task CancelEditing()
    {
      return OnAppointmentChanged.InvokeAsync(Appointment.Title);
    }

    private void StartChanged(DateTime userChoice)
    {
      if (userChoice > GetHigherDate())
      {
        ShowErrorMessage = true;
      }
      else
      {
        Appointment.Start = userChoice;
        ShowErrorMessage = false;
      }
    }

    private void EndChanged(DateTime userChoice)
    {
      if (userChoice < GetLowerDate())
      {
        ShowErrorMessage = true;
      }
      else
      {
        Appointment.End = userChoice;
        ShowErrorMessage = false;
      }
    }

    private DateTime GetLowerDate()
    {
      return Appointment.Start <= Appointment.End ? Appointment.Start : Appointment.End;
    }

    private DateTime GetHigherDate()
    {
      return Appointment.Start >= Appointment.End ? Appointment.Start : Appointment.End;
    }

    private void AppointmentTypeSelected(int? id)
    {
      Appointment.AppointmentTypeId = id ?? 0;
    }
  }
}
