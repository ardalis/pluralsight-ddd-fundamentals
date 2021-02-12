using System;

namespace VetClinicPublic.Web.Models
{
    public class AppointmentDTO
    {
        public Guid AppointmentId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmailAddress { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string AppointmentType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}