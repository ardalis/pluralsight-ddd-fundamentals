using System;

namespace BlazorShared.Models.Patient
{
  public class UpdatePatientResponse : BaseResponse
  {
    public PatientDto Patient { get; set; } = new PatientDto();

    public UpdatePatientResponse(Guid correlationId) : base(correlationId)
    {
    }

    public UpdatePatientResponse()
    {
    }
  }
}