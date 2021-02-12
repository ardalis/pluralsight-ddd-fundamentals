using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models.Room;
using ClinicManagement.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClinicManagement.Blazor.Pages
{
  public partial class RoomsPage
  {
    [Inject]
    IJSRuntime JSRuntime { get; set; }

    [Inject]
    RoomService RoomService { get; set; }

    private RoomDto ToSave = new RoomDto();
    private List<RoomDto> Rooms = new List<RoomDto>();

    protected override async Task OnInitializedAsync()
    {
      await ReloadData();
    }

    private void CreateClick()
    {
      if (Rooms.Count == 0 || Rooms[Rooms.Count - 1].RoomId != 0)
      {
        ToSave = new RoomDto();
        Rooms.Add(ToSave);
      }
    }

    private void EditClick(int id)
    {
      ToSave = Rooms.Find(x => x.RoomId == id);
    }

    private async Task DeleteClick(int id)
    {
      bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
      if (confirmed)
      {
        await RoomService.DeleteAsync(id);
        await ReloadData();
      }
    }

    private async Task SaveClick()
    {
      if (ToSave.RoomId == 0)
      {
        var toCreate = new CreateRoomRequest()
        {
          Name = ToSave.Name,
        };
        await RoomService.CreateAsync(toCreate);
      }
      else
      {
        var toUpdate = new UpdateRoomRequest()
        {
          RoomId = ToSave.RoomId,
          Name = ToSave.Name,
        };
        await RoomService.EditAsync(toUpdate);
      }

      CancelClick();
      await ReloadData();
    }

    private void CancelClick()
    {
      if (ToSave.RoomId == 0)
      {
        Rooms.RemoveAt(Rooms.Count - 1);
      }
      ToSave = new RoomDto();
    }

    private bool IsAddOrEdit(int id)
    {
      return ToSave.RoomId == id;
    }

    private async Task ReloadData()
    {
      Rooms = await RoomService.ListAsync();
    }
  }
}
