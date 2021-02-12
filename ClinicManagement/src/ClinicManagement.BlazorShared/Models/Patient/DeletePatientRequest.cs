namespace BlazorShared.Models.Patient
{
  public class DeletePatientRequest : BaseRequest
  {
    public int ClientId { get; set; }
    public int PatientId { get; set; }
  }
}
