namespace BlazorShared.Models.Patient
{
  public class GetByIdPatientRequest : BaseRequest
  {
    public int PatientId { get; set; }
  }
}
