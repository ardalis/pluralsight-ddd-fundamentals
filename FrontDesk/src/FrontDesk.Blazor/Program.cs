using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorShared;
using FrontDesk.Blazor.Services;
using FrontDesk.Blazor.Shared.SchedulerComponent;
using FrontDesk.Blazor.Shared.ToastComponent;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrontDesk.Blazor
{
  public class Program
  {
    public static Task Main(string[] args)
    {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);
      builder.RootComponents.Add<App>("#app");

      var baseUrlConfig = new BaseUrlConfiguration();
      builder.Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);
      builder.Services.AddScoped(sp => baseUrlConfig);

      // register the HttpClient and HttpService
      builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrlConfig.ApiBase) });
      builder.Services.AddScoped<HttpService>();

      // register the services
      builder.Services.AddScoped<DoctorService>();
      builder.Services.AddScoped<ClientService>();
      builder.Services.AddScoped<PatientService>();
      builder.Services.AddScoped<RoomService>();
      builder.Services.AddScoped<AppointmentService>();
      builder.Services.AddScoped<ScheduleService>();
      builder.Services.AddScoped<AppointmentTypeService>();
      builder.Services.AddScoped<FileService>();
      builder.Services.AddScoped<ConfigurationService>();
      builder.Services.AddScoped<ToastService>();
      builder.Services.AddScoped<SchedulerService>();

      // Blazor WebAssembly
      builder.Services.AddBlazoredLocalStorage();

      // register the Telerik services
      builder.Services.AddTelerikBlazor();

      return builder.Build().RunAsync();
    }
  }
}
