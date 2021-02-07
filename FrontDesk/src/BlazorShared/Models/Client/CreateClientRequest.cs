namespace BlazorShared.Models.Client
{
  public class CreateClientRequest : BaseRequest
  {
    public string FullName { get; set; }
    public string EmailAddress { get; set; }
    public string Salutation { get; set; }
    public string PreferredName { get; set; }
    public int? PreferredDoctorId { get; set; }

    //TODO: need to check
    //public IList<int> Patients { get; set; } = new List<int>();
  }
}
