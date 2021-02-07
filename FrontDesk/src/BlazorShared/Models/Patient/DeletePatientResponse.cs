using System;

namespace BlazorShared.Models.Patient
{
  public class DeletePatientResponse : BaseResponse
  {

    public DeletePatientResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeletePatientResponse()
    {
    }
  }
}