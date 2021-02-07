using System;

namespace FrontDesk.Blazor.Shared.ToastComponent
{
  public class ToastService
  {
    public string Message { get; set; } = string.Empty;
    public bool IsShow { get; set; }
    public ToastType ToastType { get; set; } = ToastType.Success;


    public event Action RefreshRequested;

    public void SendMessage(string message, ToastType toastType = ToastType.Success)
    {
      Message = message;
      IsShow = true;
      ToastType = toastType;

      RefreshRequested?.Invoke();

      new System.Threading.Timer((_) =>
      {
        this.IsShow = false;
        RefreshRequested?.Invoke();
      }, null, 5000, System.Threading.Timeout.Infinite);
    }
  }
}
