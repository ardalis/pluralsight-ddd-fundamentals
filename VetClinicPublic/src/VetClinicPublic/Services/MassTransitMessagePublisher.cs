using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MassTransit;
using VetClinicPublic.Web.Interfaces;

namespace VetClinicPublic.Web.Services;

public class MassTransitMessagePublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
{
  private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

  public Task Publish(object applicationEvent)
  {
    Guard.Against.Null(applicationEvent);
    return _publishEndpoint.Publish(applicationEvent);
  }
}
