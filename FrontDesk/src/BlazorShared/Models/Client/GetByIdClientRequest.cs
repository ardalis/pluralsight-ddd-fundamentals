namespace BlazorShared.Models.Client
{
  public class GetByIdClientRequest : BaseRequest
  {
    public const string Route = "api/clients/{ClientId}";

    public int ClientId { get; set; }
  }
}
