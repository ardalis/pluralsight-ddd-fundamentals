using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models;
using BlazorShared.Models.File;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FrontDesk.Api.FileEndpoints
{
  public class Read : Endpoint<ReadFileRequest, Results<Ok<FileItem>, BadRequest, NotFound>>
  {
    private readonly ILogger<Read> _logger;

    public Read(ILogger<Read> logger)
    {
      _logger = logger;
    }

    public override void Configure()
    {
      Get("api/files/{fileName}");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Reads a file")
           .WithDescription("Reads a file")
           .WithName("files.read")
           .WithTags("FileEndpoints"));
    }

    public override async Task<Results<Ok<FileItem>, BadRequest, NotFound>> ExecuteAsync(ReadFileRequest request,
      CancellationToken cancellationToken)
    {
      var fileName = request.FileName;
      if (string.IsNullOrEmpty(fileName)) return TypedResults.BadRequest();

      var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "patients", fileName.ToLower());
      if (!System.IO.File.Exists(fullPath))
      {
        _logger.LogError($"File not found: {fullPath}");
        return TypedResults.NotFound();
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

      return TypedResults.Ok(response);
    }
  }
}
