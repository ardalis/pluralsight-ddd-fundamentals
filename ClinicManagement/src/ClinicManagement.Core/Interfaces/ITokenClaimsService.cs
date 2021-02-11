using System.Threading.Tasks;

namespace ClinicManagement.Core.Interfaces
{
  public interface ITokenClaimsService
  {
    Task<string> GetTokenAsync(string userName);
  }
}
