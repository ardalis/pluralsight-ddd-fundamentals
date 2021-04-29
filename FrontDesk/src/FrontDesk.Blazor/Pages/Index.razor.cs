using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
    ScheduleService ScheduleService { get; set; }

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

    [Inject]
    ISyncLocalStorageService LocalStorage { get; set; }

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
    private Guid ScheduleId { get; set; } = Guid.Empty;

    private bool CanMakeAppointment => IsRoomSelected && IsClientSelected && IsPatientSelected;
    private bool IsRoomSelected => RoomId > 0;
    private bool IsClientSelected => ClientId > 0;
    private bool IsPatientSelected => PatientId > 0;

    private HubConnection hubConnection;
    private string SignalRUrl => BaseUrlConfiguration.ApiBase.Replace("api/", string.Empty);

    public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;

    private ScheduleState _currentScheduleState = new();

    protected override async Task OnInitializedAsync()
    {
      Logger.LogInformation("OnInitializedAsync()");

      var schedule = (await ScheduleService.ListAsync()).Single();
      Logger.LogInformation($"Loaded schedule: {schedule.Id}");
      ScheduleId = schedule.Id;

      SchedulerService.Appointments = await AppointmentService.ListAsync(ScheduleId);
      AppointmentTypes = await AppointmentTypeService.ListAsync();
      Clients = await ClientService.ListAsync();
      Rooms = await RoomService.ListAsync();

      Today = await ConfigurationService.ReadAsync();
      StartDate = UpdateDateToToday(StartDate);
      DayStart = UpdateDateToToday(DayStart);
      DayEnd = UpdateDateToToday(DayEnd);

      Groups.Add("Rooms");

      // check for saved client/patient Id
      await LoadScheduleState();

      IsLoaded = true;

      await InitSignalR();
    }

    protected async Task ClientChanged(object selectedClientId)
    {
      try
      {
        if (selectedClientId == null || ((int)selectedClientId) == 0)
        {
          // reset UI
          Patients = new List<PatientDto>();
          return;
        }
        ClientId = (int)selectedClientId;
        Logger.LogInformation($"Client changed: {ClientId}");

        await GetClientPatientsAsync();
      }
      finally
      {
        if (ClientId != _currentScheduleState.SelectedClientId)
        {
          SaveScheduleState();
        }
      }
    }

    private async Task GetClientPatientsAsync()
    {
      Logger.LogInformation($"Getting patients for client id {ClientId}");

      Patients = await PatientService.ListAsync(ClientId);
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

    private void PatientChanged(object id)
    {
      try
      {
        if (id == null)
        {
          SelectedPatient = null;
          return;
        }
        PatientId = (int)id;
        if (PatientId > 0)
        {
          SelectedPatient = Patients.FirstOrDefault(p => p.PatientId == PatientId);
        }
      }
      finally
      {
        SaveScheduleState();
      }
    }

    private Task InitSignalR()
    {
      hubConnection = new HubConnectionBuilder()
          .WithUrl(new Uri($"{SignalRUrl}schedulehub"))
          .Build();

      hubConnection.On<string>("ReceiveMessage", async (message) =>
      {
        Logger.LogInformation($"ReceiveMessage: {message}");
        if (message.Contains("Client") && message.Contains("updated"))
        {
          await RefreshClientsAsync();
        }
        await RefreshAppointmentsAsync();
        ToastService.SendMessage(message);
      });

      return hubConnection.StartAsync();
    }

    private async Task RefreshClientsAsync()
    {
      Clients = await ClientService.ListAsync();
    }

    private Task CancelEditing()
    {
      return RefreshAppointmentsAsync();
    }

    private async Task RefreshAppointmentsAsync()
    {
      Logger.LogInformation("RefreshDataAsync()...");
      //an event callback needs to be raised in this component context to re-render the contents and to hide the dialog
      CustomEditFormShown = false;
      CurrentAppointment = null;
      var appointments = await AppointmentService.ListAsync(ScheduleId);
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
        // convert to EDT time (UTC-4)
        var startDate = new DateTimeOffset(args.Start, new TimeSpan(-4, 0, 0));
        Logger.LogInformation($"[Index.razor.cs] Creating a new appointment for {SelectedPatient.Name} at {args.Start}");

        CurrentAppointment = new AppointmentDto() { Start = startDate, End = args.End, IsAllDay = args.IsAllDay };
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
      // convert to EDT time (UTC-4)
      int offset = -4;
      appointment.Start = new DateTimeOffset(appointment.Start.DateTime.AddHours(offset), new TimeSpan(offset, 0, 0));
      appointment.End = new DateTimeOffset(appointment.End.DateTime.AddHours(offset), new TimeSpan(offset, 0, 0));
      Logger.LogInformation($"OpenEdit called for {appointment}");

      CurrentAppointment = appointment;
      if (CurrentAppointment.AppointmentId == Guid.Empty)
      {
        if (CanMakeAppointment)
        {
          CustomEditFormShown = true;
        }
        return;
      }
      CustomEditFormShown = true;
      SaveScheduleState();
    }

    public void Dispose()
    {
      _ = hubConnection.DisposeAsync();
    }

    public class ScheduleState
    {
      public int SelectedClientId { get; set; }
      public int SelectedPatientId { get; set; }

      public override string ToString()
      {
        return $"C{SelectedClientId}-P{SelectedPatientId}";
      }
    }

    private void SaveScheduleState()
    {
      _currentScheduleState.SelectedClientId = ClientId;
      _currentScheduleState.SelectedPatientId = PatientId;

      Logger.LogInformation($"SaveScheduleState: {_currentScheduleState}");
      LocalStorage.SetItem(nameof(ScheduleState), _currentScheduleState);
    }

    private async Task LoadScheduleState()
    {
      var currentState = LocalStorage.GetItem<ScheduleState>(nameof(ScheduleState));

      if (currentState == null) return;

      _currentScheduleState = currentState;
      await ClientChanged(_currentScheduleState.SelectedClientId);

      if (_currentScheduleState.SelectedPatientId > 0)
      {
        PatientId = _currentScheduleState.SelectedPatientId;
        SelectedPatient = Patients.FirstOrDefault(p => p.PatientId == PatientId);
      }
    }
  }
}
