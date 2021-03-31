using System;

namespace VetClinicPublic.Web.Models
{
    public class AppointmentConfirmedEvent
    {
        public AppointmentConfirmedEvent(Guid appointmentId)
        {
            this.Id = Guid.NewGuid();
            DateTimeEventOccurred = DateTime.Now;
            this.AppointmentId = appointmentId;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset DateTimeEventOccurred { get; set; }
        public Guid AppointmentId { get; set; }
        public string EventType
        {
            get
            {
                return "AppointmentConfirmedEvent";
            }
        }
    }
}
