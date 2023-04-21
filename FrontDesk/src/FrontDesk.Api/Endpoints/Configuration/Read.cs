using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.ConfigurationEndpoints
{
  public class Read : BaseAsyncEndpoint
    .WithoutRequest
    .WithResponse<string>
  {
    public Read()
    {
    }

    [HttpGet("api/configurations")]
    [SwaggerOperation(
        Summary = "Read configuration settings",
        Description = "Read configuration settingss",
        OperationId = "Configurations.Read",
        Tags = new[] { "ConfigurationEndpoints" })
    ]
    public override async Task<ActionResult<string>> HandleAsync(CancellationToken cancellationToken)
    {
      return Ok(new OfficeSettings().TestDate.ToString(CultureInfo.InvariantCulture));
    }
  }
}
