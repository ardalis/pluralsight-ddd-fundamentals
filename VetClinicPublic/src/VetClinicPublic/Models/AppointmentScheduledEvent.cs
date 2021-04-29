using System;

namespace VetClinicPublic.Web.Models
{
    public class AppointmentScheduledEvent
    {
        public AppointmentScheduledEvent(SendAppointmentConfirmationCommand appointment) : this()
        {
            AppointmentScheduled = appointment;
        }

        public AppointmentScheduledEvent()
        {
            DateTimeEventOccurred = DateTime.Now;
        }

        public DateTime DateTimeEventOccurred { get; set; }
        public SendAppointmentConfirmationCommand AppointmentScheduled { get; set; }
        public string EventType
        {
            get
            {
                return "AppointmentScheduledEvent";
            }
        }
    }

}