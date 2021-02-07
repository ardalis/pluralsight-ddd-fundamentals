using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using BlazorShared.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.FileEndpoints
{
  public class Read : BaseAsyncEndpoint<string, FileItem>
  {
    public Read()
    {
    }

    [HttpGet("api/files/{fileName}")]
    [SwaggerOperation(
        Summary = "Reads a file",
        Description = "Reads a file",
        OperationId = "files.read",
        Tags = new[] { "FileEndpoints" })
    ]
    public override async Task<ActionResult<FileItem>> HandleAsync([FromRoute] string fileName, CancellationToken cancellationToken)
    {
      if (string.IsNullOrEmpty(fileName)) return BadRequest();

      var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "patients", fileName.ToLower());
      if (!System.IO.File.Exists(fullPath))
      {
        // TODO: Add logger
        System.Console.WriteLine($"File not found: {fullPath}");
        return NotFound();
      }

      byte[] fileArray = System.IO.File.ReadAllBytes(fullPath);
      if (fileArray.Length <= 0) return BadRequest();

      string fileDataBase64 = Convert.ToBase64String(fileArray);
      var respons = new FileItem()
      {
        DataBase64 = fileDataBase64,
        FileName = fileName
      };

      return Ok(respons);
    }
  }
}
