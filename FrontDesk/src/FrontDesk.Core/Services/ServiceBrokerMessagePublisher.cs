using System;
using FrontDesk.Core.Interfaces;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Core.Services
{
  public class ServiceBrokerMessagePublisher: IMessagePublisher
  {
    public void Publish(IApplicationEvent applicationEvent)
    {
      throw new NotImplementedException();
    }
  }
}
