using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.ConfigurationEndpoints
{
  public class Read : BaseEndpoint
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
    public override ActionResult<string> Handle()
    {
      return Ok(new OfficeSettings().TestDate.ToString());
    }
  }
}
