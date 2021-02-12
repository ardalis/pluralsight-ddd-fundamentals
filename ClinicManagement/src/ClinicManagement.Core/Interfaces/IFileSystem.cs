using System.Threading.Tasks;

namespace ClinicManagement.Core.Interfaces
{
  public interface IFileSystem
  {
    Task<bool> SavePicture(string pictureName, string pictureBase64);
  }
}
