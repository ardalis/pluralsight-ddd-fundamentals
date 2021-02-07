using System.Threading.Tasks;

namespace FrontDesk.Core.Interfaces
{
  public interface IFileSystem
  {
    Task<bool> SavePicture(string pictureName, string pictureBase64);
  }
}
