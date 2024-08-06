using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ClinicManagement.Api.ConfigurationEndpoints
{
  public class Read : Endpoint<EmptyRequest, string>
  {
    public Read()
    {
    }

    public override void Configure()
    {
      Get("api/configurations");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Read configuration settings")
           .WithDescription("Read configuration settings")
           .WithName("Configurations.Read")
           .WithTags("ConfigurationEndpoints"));
    }

    public override Task<string> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
      return Task.FromResult(new OfficeSettings().TestDate.ToString());
    }
  }
}
