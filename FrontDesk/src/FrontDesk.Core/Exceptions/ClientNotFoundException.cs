using System;

namespace FrontDesk.Core.Exceptions
{
  public class ClientNotFoundException : Exception
  {
    public ClientNotFoundException(string message) : base(message)
    {
    }

    public ClientNotFoundException(int clientId) : base($"No client with id {clientId} found.")
    {
    }
  }
}
