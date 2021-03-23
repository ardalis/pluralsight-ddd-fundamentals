using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BlazorShared;
using BlazorShared.Models.Appointment;
using BlazorShared.Models.AppointmentType;
using BlazorShared.Models.Client;
using BlazorShared.Models.Patient;
using BlazorShared.Models.Room;
using FrontDesk.Blazor.Services;
using FrontDesk.Blazor.Shared.SchedulerComponent;
using FrontDesk.Blazor.Shared.ToastComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Telerik.Blazor.Components;

namespace FrontDesk.Blazor.Pages
{
  public partial class Index
  {
    [Inject]
    ILogger<Index> Logger { get; set; }

    [Inject]
    AppointmentService AppointmentService { get; set; }

    [Inject]
    AppointmentTypeService AppointmentTypeService { get; set; }

    [Inject]
    RoomService RoomService { get; set; }

    [Inject]
    PatientService PatientService { get; set; }

    [Inject]
    ClientService ClientService { get; set; }

    [Inject]
    FileService FileService { get; set; }

    [Inject]
    ConfigurationService ConfigurationService { get; set; }

    [Inject]
    BaseUrlConfiguration BaseUrlConfiguration { get; set; }

    [Inject]
    ToastService ToastService { get; set; }

    [Inject]
    SchedulerService SchedulerService { get; set; }

    private bool IsShowEdit = false;
    private bool IsLoaded = false;
    private List<string> Groups = new List<string>();
    private List<AppointmentTypeDto> AppointmentTypes = new List<AppointmentTypeDto>();
    private List<ClientDto> Clients = new List<ClientDto>();
    private List<RoomDto> Rooms = new List<RoomDto>();
    private List<PatientDto> Patients = new List<PatientDto>();

    private bool CustomEditFormShown { get; set; }
    AppointmentDto CurrentAppointment { get; set; } // we will put here a copy of the appointment for editing

    private DateTime Today { get; set; } = new DateTime();
    private DateTime StartDate { get; set; } = new DateTime(2014, 6, 9, 7, 0, 0);
    private DateTime DayStart { get; set; } = new DateTime(2014, 6, 9, 7, 0, 0);
    private DateTime DayEnd { get; set; } = new DateTime(2014, 6, 9, 18, 00, 0);

    private int PatientId { get; set; } = 0;
    private int ClientId { get; set; } = 0;
    private int RoomId { get; set; } = 1;
    private PatientDto SelectedPatient { get; set; } = new PatientDto();
    private Guid ScheduleId
    {
      get
      {
        if (SchedulerService.Appointments.Count > 0)
        {
          return SchedulerService.Appointments[0].ScheduleId;
        }

        return Guid.Empty;
      }
    }

    private bool CanMakeAppointment => IsRoomSelected && IsClientSelected && IsPatientSelected;
    private bool IsRoomSelected => RoomId > 0;
    private bool IsClientSelected => ClientId > 0;
    private bool IsPatientSelected => PatientId > 0;

    private HubConnection hubConnection;
    private string SignalRUrl => BaseUrlConfiguration.ApiBase.Replace("api/", string.Empty);

    public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;

    protected override async Task OnInitializedAsync()
    {
      Logger.LogInformation("OnInitializedAsync()");
      SchedulerService.Appointments = await AppointmentService.ListAsync();
      AppointmentTypes = await AppointmentTypeService.ListAsync();
      Clients = await ClientService.ListAsync();
      Rooms = await RoomService.ListAsync();

      // Patients belong to Clients - This should be driven by the selected client
      //await GetClientPatientsAsync();
      //Patients = await PatientService.ListAsync(ClientId);
      //Patient = Patients.FirstOrDefault(p => p.PatientId == PatientId);

      Today = await ConfigurationService.ReadAsync();
      StartDate = UpdateDateToToday(StartDate);
      DayStart = UpdateDateToToday(DayStart);
      DayEnd = UpdateDateToToday(DayEnd);

      Groups.Add("Rooms");

      IsLoaded = true;

      await InitSignalR();
    }

    protected void ClientChanged(int? clientId)
    {
      if(clientId == null)
      {
        // reset UI
        return;
      }
      ClientId = clientId.Value;
      Logger.LogInformation($"Client changed: {ClientId}");

      GetClientPatientsAsync().GetAwaiter().GetResult();
    }

    private async Task GetClientPatientsAsync()
    {
      Logger.LogInformation($"Getting patients for client id {ClientId}");

      Patients = await PatientService.ListAsync(ClientId);
      //Patients.Clear();
      //Patients.AddRange(await PatientService.ListAsync(ClientId));
      await AddPatientImages();
      SelectedPatient = Patients.FirstOrDefault();
      Logger.LogInformation($"Current patient: {SelectedPatient.Name} ({SelectedPatient.PatientId})");
    }

    private async Task AddPatientImages()
    {
      foreach (var patient in Patients)
      {
        if (string.IsNullOrEmpty(patient.Name))
        {
          continue;
        }

        var imgData = await FileService.ReadPicture($"{patient.Name.ToLower()}.jpg");
        if (string.IsNullOrEmpty(imgData))
        {
          continue;
        }

        patient.ImageData = $"data:image/png;base64, {imgData}";
        Logger.LogInformation($"Loaded image data for {patient.Name}");
      }
    }

    private void PatientChanged(int id)
    {
      PatientId = id;
      if (PatientId > 0)
      {
        SelectedPatient = Patients.FirstOrDefault(p => p.PatientId == PatientId);
      }
    }



    private Task InitSignalR()
    {
      hubConnection = new HubConnectionBuilder()
          .WithUrl(new Uri($"{SignalRUrl}schedulehub"))
          .Build();

      hubConnection.On<string>("ReceiveMessage", async (message) =>
      {
        await RefreshDataAsync();
        ToastService.SendMessage(message);
      });

      return hubConnection.StartAsync();
    }

    private Task CancelEditing()
    {
      return RefreshDataAsync();
    }

    private async Task RefreshDataAsync()
    {
      //an event callback needs to be raised in this component context to re-render the contents and to hide the dialog
      CustomEditFormShown = false;
      CurrentAppointment = null;
      //we also use it to fetch the fresh data from the service - in a real case other updates may have occurred
      //which is why I chose to use a separate event and not two-way binding. It is also used for refreshing on Cancel
      var appointments = await AppointmentService.ListAsync();

      SchedulerService.RefreshAppointments(appointments);
    }

    private void EditHandler(SchedulerEditEventArgs args)
    {
      args.IsCancelled = true;//prevent built-in edit form from showing up
      if (!CanMakeAppointment)
      {
        return;
      }

      AppointmentDto item = args.Item as AppointmentDto;
      CustomEditFormShown = true;
      if (!args.IsNew) // an edit operation, otherwise - an insert operation
      {
        //copying is implemented in the appointment model and it is needed because
        //this is a reference to the data collection so modifying it directly
        //will immediately modify the data and no cancelling will be possible
        CurrentAppointment = item.ShallowCopy();
      }
      else
      {
        CurrentAppointment = new AppointmentDto() { Start = args.Start, End = args.End, IsAllDay = args.IsAllDay };
      }
    }

    private async Task DeleteAppointmentAsync(SchedulerDeleteEventArgs args)
    {
      AppointmentDto item = (AppointmentDto)args.Item;
      await AppointmentService.DeleteAsync(item.ScheduleId, item.AppointmentId);
      SchedulerService.Appointments.Remove(item);
    }

    private DateTime UpdateDateToToday(DateTime date)
    {
      return new DateTime(Today.Year, Today.Month, Today.Day, date.Hour, date.Minute, date.Second);
    }

    private void OpenEdit(AppointmentDto appointment)
    {
      CurrentAppointment = appointment;
      CustomEditFormShown = true;
    }

    public void Dispose()
    {
      _ = hubConnection.DisposeAsync();
    }


  }
}
