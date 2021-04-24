using System;

namespace FrontDesk.Core.Exceptions
{
  public class DuplicateAppointmentException : ArgumentException
  {
    public DuplicateAppointmentException(string message, string paramName) : base(message, paramName)
    {
    }
  }
}
