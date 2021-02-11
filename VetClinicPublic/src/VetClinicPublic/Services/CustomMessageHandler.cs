using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;
using VetClinicPublic.Web.Interfaces;

namespace VetClinicPublic.Web.Services
{
  public class CustomMessageHandler : IMessageHandler
  {
    readonly ILogger<CustomMessageHandler> _logger;
    private readonly ISendConfirmationEmails _emailSender;

    public CustomMessageHandler(ILogger<CustomMessageHandler> logger,
      ISendConfirmationEmails emailSender)
    {
      _logger = logger;
      _emailSender = emailSender;
    }

    public void Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
    {
      // Do whatever you want with the message.
      _logger.LogInformation("Message Received - Sending Email!");

      System.Threading.Thread.Sleep(500);
      // send email
    }
  }

}
