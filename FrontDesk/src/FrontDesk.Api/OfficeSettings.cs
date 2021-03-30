using System;
using FrontDesk.Core.Interfaces;

namespace FrontDesk.Api
{
  // These can be read from config but for demo purposes are hard-coded here.
  public class OfficeSettings : IApplicationSettings
  {
    public int ClinicId { get { return 1; } }
    public DateTimeOffset TestDate { get 
      { 
        return new DateTimeOffset(2030, 9, 23, 0, 0, 0, new TimeSpan(-4, 0, 0));
      }
    }
  }
}
