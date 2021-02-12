using System;

namespace BlazorShared.Models.Doctor
{
  public class DeleteDoctorResponse : BaseResponse
  {

    public DeleteDoctorResponse(Guid correlationId) : base(correlationId)
    {
    }

    public DeleteDoctorResponse()
    {
    }
  }
}