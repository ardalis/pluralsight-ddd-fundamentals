namespace BlazorShared.Models.Patient
{
  public class PatientDto
  {
    public int PatientId { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string ClientName { get; set; }
    public string ImageData { get; set; }
    public int? PreferredDoctorId { get; set; }
  }
}
