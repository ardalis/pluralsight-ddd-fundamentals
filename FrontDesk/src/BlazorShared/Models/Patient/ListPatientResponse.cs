using System;
using System.Collections.Generic;

namespace BlazorShared.Models.Patient
{
  public class ListPatientResponse : BaseResponse
  {
    public List<PatientDto> Patients { get; set; } = new List<PatientDto>();

    public int Count { get; set; }

    public ListPatientResponse(Guid correlationId) : base(correlationId)
    {
    }

    public ListPatientResponse()
    {
    }
  }
}