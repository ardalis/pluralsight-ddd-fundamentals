using VetClinicPublic.Web.Models;

namespace VetClinicPublic.Web.Interfaces
{
  public interface ISendConfirmationEmails
    {
        void SendConfirmationEmail(SendAppointmentConfirmationCommand appointment);
    }
}
