using System.Threading.Tasks;
using BlazorShared;
using ClinicManagement.Blazor.Shared.ToastComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClinicManagement.Blazor.Pages
{
  public partial class Index
  {
    [Inject]
    BaseUrlConfiguration BaseUrlConfiguration { get; set; }

    [Inject]
    ToastService ToastService { get; set; }

    private bool IsLoaded = false;
    private HubConnection hubConnection;
    private string SignalRUrl => BaseUrlConfiguration.ApiBase.Replace("api/", string.Empty);

    public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;

    public void Dispose()
    {
      _ = hubConnection.DisposeAsync();
    }

    protected override Task OnInitializedAsync()
    {
      IsLoaded = true;

      return Task.CompletedTask;
      //return InitSignalR();
    }

    //private Task InitSignalR()
    //{
    //  hubConnection = new HubConnectionBuilder()
    //      .WithUrl(new Uri($"{SignalRUrl}{SignalRConstants.HUB_NAME}"))
    //      .Build();

    //  hubConnection.On<string>("ReceiveMessage", async (message) =>
    //  {
    //    await RefreshDataAsync();
    //    ToastService.SendMessage(message);
    //  });

    //  return hubConnection.StartAsync();
    //}

    // private async Task RefreshDataAsync()
    // {
    //   //an event callback needs to be raised in this component context to re-render the contents and to hide the dialog
    //   //CustomEditFormShown = false;
    //   //CurrentAppointment = null;
    //   //we also use it to fetch the fresh data from the service - in a real case other updates may have occurred
    //   //which is why I chose to use a separate event and not two-way binding. It is also used for refreshing on Cancel
    //   //var appointments = await AppointmentService.ListAsync();

    //   //SchedulerService.RefreshAppointments(appointments);
    // }


    //private async Task AddPatientImages()
    //{
    //  foreach (var patient in Patients)
    //  {
    //    if (string.IsNullOrEmpty(patient.Name))
    //    {
    //      continue;
    //    }

    //    var imgeData = await FileService.ReadPicture($"{patient.Name}.jpg");
    //    if (string.IsNullOrEmpty(imgeData))
    //    {
    //      continue;
    //    }

    //    patient.ImageData = $"data:image/png;base64, {imgeData}";
    //  }
    //}

  }
}
