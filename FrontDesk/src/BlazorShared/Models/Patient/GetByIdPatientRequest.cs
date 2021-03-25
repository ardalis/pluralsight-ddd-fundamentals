namespace BlazorShared.Models.Patient
{
  public class GetByIdPatientRequest : BaseRequest
  {
    public const string Route = "api/patients/{PatientId}/byclient/{ClientId}";
    public int ClientId { get; set; }
    public int PatientId { get; set; }
  }
}
