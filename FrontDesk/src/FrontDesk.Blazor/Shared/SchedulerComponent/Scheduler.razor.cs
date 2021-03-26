using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorShared.Models.Appointment;
using BlazorShared.Models.AppointmentType;
using BlazorShared.Models.Room;
using FrontDesk.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FrontDesk.Blazor.Shared.SchedulerComponent
{

  public partial class Scheduler
  {
    [Inject]
    IJSRuntime JSRuntime { get; set; }

    [Inject]
    AppointmentService AppointmentService { get; set; }

    [Inject]
    SchedulerService SchedulerService { get; set; }

    [Parameter]
    public int Height { get; set; } = 750;
    [Parameter]
    public RenderFragment ChildContent { get; set; }
    [Parameter]
    public RenderFragment<Resource> RenderFragmentResources { get; set; }

    [Parameter] public List<string> Groups { get; set; } = new List<string>();
    [Parameter] public DateTime StartDate { get; set; } = new DateTime();
    [Parameter] public DateTime StartTime { get; set; } = new DateTime();
    [Parameter] public DateTime EndTime { get; set; } = new DateTime();
    [Parameter] public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();

    [Parameter] public List<AppointmentTypeDto> AppointmentTypes { get; set; } = new List<AppointmentTypeDto>();
    [Parameter] public List<SchedulerResourceModel> SchedulerResources { get; set; } = new List<SchedulerResourceModel>();

    [Parameter]
    public EventCallback<AppointmentDto> OnEditCallback { get; set; }

    [Parameter]
    public EventCallback OnScheduleChangedCallback { get; set; }

    private List<SchedulerResourceModel> Resources { get; set; } = new List<SchedulerResourceModel>();

    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      if (firstRender)
      {
        SchedulerService.RefreshRequested += Refresh;
        CallJSMethod();
        var thisReference = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("addListenerToFireEdit", thisReference);
      }
      await base.OnAfterRenderAsync(firstRender);
    }

    public void AddResource(Resource resourceComponent)
    {
      Resources.Add(resourceComponent.SchedulerResource);
      Refresh();
    }

    public void Refresh()
    {
      CallJSMethod();
    }

    private void CallJSMethod()
    {
      var processRuntime = JSRuntime as IJSInProcessRuntime;
      processRuntime.InvokeVoid("scheduler", StartDate, StartTime, EndTime, SchedulerService.Appointments, Resources, Groups, Height);
    }

    [JSInvokable]
    public async Task KendoCall(string action, string jsonData)
    {
      if (action == "edit")
      {
        var result = JsonSerializer.Deserialize<AppointmentDto>(jsonData, JsonOptions);
        await OnEditCallback.InvokeAsync(result);
      }
      else if (action == "move")
      {
        var result = JsonSerializer.Deserialize<AppointmentDto>(jsonData, JsonOptions);
        await AppointmentService.EditAsync(UpdateAppointmentRequest.FromDto(result));
        await OnScheduleChangedCallback.InvokeAsync();
      }
      else if (action == "delete")
      {
        var result = JsonSerializer.Deserialize<AppointmentDto>(jsonData, JsonOptions);
        await AppointmentService.DeleteAsync(result.ScheduleId, result.AppointmentId);
        SchedulerService.Appointments.Remove(SchedulerService.Appointments.First(x => x.AppointmentId == result.AppointmentId));
        CallJSMethod();
        await OnScheduleChangedCallback.InvokeAsync();
      }
    }
  }
}
