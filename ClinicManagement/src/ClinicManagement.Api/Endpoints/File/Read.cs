using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using BlazorShared.Models;
using BlazorShared.Models.Doctor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.FileEndpoints
{
  public class Read : BaseAsyncEndpoint
    .WithRequest<string>
    .WithResponse<FileItem>
  {
    private readonly ILogger<Read> _logger;

    public Read(ILogger<Read> logger)
    {
      _logger = logger;
    }

    [HttpGet("api/files/{fileName}")]
    [SwaggerOperation(
        Summary = "Reads a file",
        Description = "Reads a file",
        OperationId = "files.read",
        Tags = new[] { "FileEndpoints" })
    ]
    public override async Task<ActionResult<FileItem>> HandleAsync([FromRoute] string fileName,
      CancellationToken cancellationToken)
    {
      if (string.IsNullOrEmpty(fileName)) return BadRequest();

      var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "patients", fileName.ToLower());
      if (!System.IO.File.Exists(fullPath))
      {
        _logger.LogError($"File not found: {fullPath}");
        return NotFound();
      }

      int maxWidth = 200;
      var outputStream = new MemoryStream();
      using(var image = Image.Load(fullPath))
      {
        int newWidth = Math.Min(image.Width, maxWidth);
        _logger.LogDebug($"Resizing to: {newWidth}");

        if (newWidth != image.Width)
        {
          image.Mutate(x => x.Resize(newWidth, 0));
          _logger.LogDebug($"Resized to {image.Width} x {image.Height}");
        }
        image.Save(outputStream, new JpegEncoder());
      }

      string fileDataBase64 = Convert.ToBase64String(outputStream.ToArray());
      var response = new FileItem()
      {
        DataBase64 = fileDataBase64,
        FileName = fileName
      };

      return Ok(response);
    }
  }
}
