using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models.Configuration;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FrontDesk.Api.ConfigurationEndpoints
{
  public class Read : Endpoint<GetConfigurationRequest, string>
  {
    public Read()
    {
    }

    public override void Configure()
    {
      Get(GetConfigurationRequest.Route);
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Read configuration settings")
           .WithDescription("Read configuration settingss")
           .WithName("Configurations.Read")
           .WithTags("ConfigurationEndpoints"));
    }

    public override Task HandleAsync(GetConfigurationRequest req, CancellationToken cancellationToken)
    {
      return SendStringAsync(new OfficeSettings().TestDate.ToString(CultureInfo.InvariantCulture));
    }
  }
}
