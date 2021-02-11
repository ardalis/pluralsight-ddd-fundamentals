namespace BlazorShared.Models.Patient
{
  public class CreatePatientRequest : BaseRequest
  {
    public int ClientId { get; set; }
    public string PatientName { get; set; }
  }
}
