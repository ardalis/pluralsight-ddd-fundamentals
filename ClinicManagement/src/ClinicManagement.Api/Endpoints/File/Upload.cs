using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BlazorShared.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ClinicManagement.Api.FileEndpoints
{
  public class Upload : Endpoint<FileItem, Results<Ok<bool>, BadRequest>>
  {
    public Upload()
    {
    }

    public override void Configure()
    {
      Post("api/files");
      AllowAnonymous();
      Description(d =>
          d.WithSummary("Uploads a file")
           .WithDescription("Uploads a file")
           .WithName("files.upload")
           .WithTags("FileEndpoints"));
    }

    public override async Task<Results<Ok<bool>, BadRequest>> HandleAsync(FileItem request, CancellationToken cancellationToken)
    {
      if (request == null || string.IsNullOrEmpty(request.DataBase64)) return TypedResults.BadRequest();

      var fileData = Convert.FromBase64String(request.DataBase64);
      if (fileData.Length <= 0) return TypedResults.BadRequest();

      var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"images/Patients", request.FileName.ToLower());
      if (System.IO.File.Exists(fullPath))
      {
        System.IO.File.Delete(fullPath);
      }
      await System.IO.File.WriteAllBytesAsync(fullPath, fileData);

      return TypedResults.Ok(true);
    }
  }
}
