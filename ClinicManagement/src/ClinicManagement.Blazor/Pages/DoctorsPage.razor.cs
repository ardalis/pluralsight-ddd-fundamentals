using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models.Doctor;
using ClinicManagement.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClinicManagement.Blazor.Pages
{
  public partial class DoctorsPage
  {
    [Inject]
    IJSRuntime JSRuntime { get; set; }

    [Inject]
    DoctorService DoctorService { get; set; }

    private DoctorDto ToSave = new DoctorDto();
    private List<DoctorDto> Doctors = new List<DoctorDto>();

    protected override async Task OnInitializedAsync()
    {
      await ReloadData();
    }

    private void CreateClick()
    {
      if (Doctors.Count == 0 || Doctors[Doctors.Count - 1].DoctorId != 0)
      {
        ToSave = new DoctorDto();
        Doctors.Add(ToSave);
      }
    }

    private void EditClick(int id)
    {
      ToSave = Doctors.Find(x => x.DoctorId == id);
    }

    private async Task DeleteClick(int id)
    {
      bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
      if (confirmed)
      {
        await DoctorService.DeleteAsync(id);
        await ReloadData();
      }
    }

    private async Task SaveClick()
    {
      if (ToSave.DoctorId == 0)
      {
        var toCreate = new CreateDoctorRequest()
        {
          Name = ToSave.Name,
        };
        await DoctorService.CreateAsync(toCreate);
      }
      else
      {
        var toUpdate = new UpdateDoctorRequest()
        {
          DoctorId = ToSave.DoctorId,
          Name = ToSave.Name,
        };
        await DoctorService.EditAsync(toUpdate);
      }

      CancelClick();
      await ReloadData();
    }

    private void CancelClick()
    {
      if (ToSave.DoctorId == 0)
      {
        Doctors.RemoveAt(Doctors.Count - 1);
      }
      ToSave = new DoctorDto();
    }

    private bool IsAddOrEdit(int id)
    {
      return ToSave.DoctorId == id;
    }

    private async Task ReloadData()
    {
      Doctors = await DoctorService.ListAsync();
    }
  }
}
