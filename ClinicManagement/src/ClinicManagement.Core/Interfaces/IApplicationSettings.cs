using System;

namespace ClinicManagement.Core.Interfaces
{
  public interface IApplicationSettings
  {
    int ClinicId { get; }
    DateTime TestDate { get; }
  }
}