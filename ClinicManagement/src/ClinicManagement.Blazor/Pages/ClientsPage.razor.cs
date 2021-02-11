using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models.Client;
using ClinicManagement.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClinicManagement.Blazor.Pages
{
  public partial class ClientsPage
  {
    [Inject]
    IJSRuntime JSRuntime { get; set; }

    [Inject]
    ClientService ClientService { get; set; }

    private ClientDto ToSave = new ClientDto();
    private List<ClientDto> Clients = new List<ClientDto>();

    protected override async Task OnInitializedAsync()
    {
      await ReloadData();
    }

    private void CreateClick()
    {
      if (Clients.Count == 0 || Clients[Clients.Count - 1].ClientId != 0)
      {
        ToSave = new ClientDto();
        Clients.Add(ToSave);
      }
    }

    private void EditClick(int id)
    {
      ToSave = Clients.Find(x => x.ClientId == id);
    }

    private async Task DeleteClick(int id)
    {
      bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
      if (confirmed)
      {
        await ClientService.DeleteAsync(id);
        await ReloadData();
      }
    }

    private async Task SaveClick()
    {
      if (ToSave.ClientId == 0)
      {
        var toCreate = new CreateClientRequest()
        {
          FullName = ToSave.FullName,
          EmailAddress = ToSave.EmailAddress,
          Salutation = ToSave.Salutation,
          PreferredDoctorId = ToSave.PreferredDoctorId,
          PreferredName = ToSave.PreferredName,
        };
        await ClientService.CreateAsync(toCreate);
      }
      else
      {
        var toUpdate = new UpdateClientRequest()
        {
          ClientId = ToSave.ClientId,
          FullName = ToSave.FullName,
          EmailAddress = ToSave.EmailAddress,
          Salutation = ToSave.Salutation,
          PreferredDoctorId = ToSave.PreferredDoctorId,
          PreferredName = ToSave.PreferredName,
        };

        await ClientService.EditAsync(toUpdate);
      }

      CancelClick();
      await ReloadData();
    }

    private void CancelClick()
    {
      if (ToSave.ClientId == 0)
      {
        Clients.RemoveAt(Clients.Count - 1);
      }
      ToSave = new ClientDto();
    }

    private bool IsAddOrEdit(int id)
    {
      return ToSave.ClientId == id;
    }

    private async Task ReloadData()
    {
      Clients = await ClientService.ListAsync();
    }
  }
}
