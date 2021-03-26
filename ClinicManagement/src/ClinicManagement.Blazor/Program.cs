using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorShared;
using ClinicManagement.Blazor.Services;
using ClinicManagement.Blazor.Shared.ToastComponent;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Blazor
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
      builder.Services.AddScoped<AppointmentTypeService>();
      builder.Services.AddScoped<FileService>();
      builder.Services.AddScoped<ConfigurationService>();
      builder.Services.AddScoped<ToastService>();

      return builder.Build().RunAsync();
    }
  }
}
