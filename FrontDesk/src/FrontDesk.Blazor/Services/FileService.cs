using System.Threading.Tasks;
using BlazorShared.Models;

namespace FrontDesk.Blazor.Services
{
  public class FileService
  {
    private readonly HttpService _httpService;

    public FileService(HttpService httpService)
    {
      _httpService = httpService;
    }

    public async Task<string> ReadPicture(string pictureName)
    {
      if (string.IsNullOrEmpty(pictureName))
      {
        return null;
      }
      var fileItem = await _httpService.HttpGetAsync<FileItem>($"api/files/{pictureName}");

      return fileItem == null ? null : fileItem.DataBase64;
    }
  }
}
