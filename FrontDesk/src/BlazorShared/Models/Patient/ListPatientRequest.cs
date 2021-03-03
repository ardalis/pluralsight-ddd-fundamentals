namespace BlazorShared.Models.Patient
{
  public class ListPatientRequest : BaseRequest
  {
    public const string Route = "api/patients/byclient/{clientId}";
    public int ClientId { get; set; }
  }
}
