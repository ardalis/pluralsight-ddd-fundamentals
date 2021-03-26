using System.Net.Mail;
using Microsoft.Extensions.Options;
using VetClinicPublic.Web.Interfaces;

namespace VetClinicPublic.Web.Services
{
  public class SmtpEmailSender : ISendEmail
  {
    private readonly MailserverConfiguration _config;

    public SmtpEmailSender(
      IOptions<MailserverConfiguration> mailserverOptions)
    {
      _config = mailserverOptions.Value;
    }

    public void SendEmail(string to, string from, string subject, string body)
    {
      using var client = new SmtpClient(_config.Hostname, _config.Port);
      var mailMessage = new MailMessage();
      mailMessage.To.Add(to);
      mailMessage.From = new MailAddress(from);
      mailMessage.Subject = subject;
      mailMessage.IsBodyHtml = true;
      mailMessage.Body = body;
      client.Send(mailMessage);
    }
  }
}
