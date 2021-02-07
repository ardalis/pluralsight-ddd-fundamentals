using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.ConfigurationEndpoints
{
  public class Read : BaseAsyncEndpoint<string>
  {
    public Read()
    {
    }

    [HttpGet("api/configurations")]
    [SwaggerOperation(
        Summary = "Read configurations",
        Description = "Read configurations",
        OperationId = "configurations.read",
        Tags = new[] { "configurationEndpoints" })
    ]
    public override async Task<ActionResult<string>> HandleAsync(CancellationToken cancellationToken)
    {
      return Ok(new OfficeSettings().TestDate.ToString());
    }
  }
}
