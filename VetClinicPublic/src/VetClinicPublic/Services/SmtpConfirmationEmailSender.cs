using System;
using System.Net.Mail;
using VetClinicPublic.Web.Interfaces;

namespace VetClinicPublic.Web.Services
{
  /// <summary>
  /// Emails are sent from the public site because that's where the SMTP server is in this example.
  /// This is mainly just to demonstrate that messages can pass both to and from this system and other systems.
  /// </summary>
  public class SmtpConfirmationEmailSender : ISendConfirmationEmails
  {
    public void SendConfirmationEmail(Models.AppointmentDTO appointment)
    {
      using (var client = new SmtpClient("localhost"))
      {
        var mailMessage = new MailMessage();
        mailMessage.To.Add(appointment.ClientEmailAddress);
        mailMessage.From = new MailAddress("donotreply@thevetclinic.com");
        mailMessage.Subject = "Vet Appointment Confirmation for " + appointment.PatientName;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = String.Format("<html><body>Dear {0},<br/><p>Please click the link below to confirm {1}'s appointment for a {2} with {3} on {4}.</p><p>Thanks!</p><p><a href='{5}'>CONFIRM</a></p><p>Please call the office to reschedule if you will be unable to make it for your appointment.</p><p>Have a great day!</p></body></html>", appointment.ClientName, appointment.PatientName, appointment.AppointmentType, appointment.DoctorName, appointment.Start.ToString(), "http://localhost:51322/appointment/confirm/" + appointment.AppointmentId);
        client.Send(mailMessage);
      }
    }
  }
}
