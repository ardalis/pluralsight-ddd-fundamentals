using System;

namespace FrontDesk.Core.Interfaces
{
  public interface IApplicationSettings
  {
    int ClinicId { get; }
    DateTime TestDate { get; }
  }
}