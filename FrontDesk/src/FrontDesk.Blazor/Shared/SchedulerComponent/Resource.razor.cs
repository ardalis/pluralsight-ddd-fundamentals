using Microsoft.AspNetCore.Components;

namespace FrontDesk.Blazor.Shared.SchedulerComponent
{
  public partial class Resource
  {
    [Parameter] public string Name { get; set; } = string.Empty;
    [Parameter] public string TextField { get; set; } = string.Empty;
    [Parameter] public string ValueField { get; set; } = string.Empty;
    [Parameter] public string Field { get; set; } = string.Empty;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public object Data { get; set; } = new object();

    [CascadingParameter]
    private Scheduler Parent { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    public SchedulerResourceModel SchedulerResource => new SchedulerResourceModel(Name, TextField, ValueField, Field, Title, Data);

    protected override void OnInitialized()
    {
      Parent.AddResource(this);
      base.OnInitialized();
    }
  }
}
