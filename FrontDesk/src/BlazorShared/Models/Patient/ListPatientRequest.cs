namespace BlazorShared.Models.Patient
{
  public class ListPatientRequest : BaseRequest
  {
    public const string Route = "api/patients/byclient/{ClientId}";
    public int ClientId { get; set; }
  }
}
