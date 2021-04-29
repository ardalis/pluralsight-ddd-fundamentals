using System;
using MediatR;

namespace VetClinicPublic.Web.Models
{
    public class SendAppointmentConfirmationCommand : IRequest
    {
        public Guid AppointmentId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmailAddress { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string AppointmentType { get; set; }
        public DateTime AppointmentStartDateTime { get; set; }
    }
}
