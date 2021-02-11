using System;
using System.Linq;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic.Web.Interfaces
{
    public interface ISendConfirmationEmails
    {
        void SendConfirmationEmail(AppointmentDTO appointment);
    }
}
