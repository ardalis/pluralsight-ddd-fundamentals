using MediatR;

namespace FrontDesk.Infrastructure.Data.Sync
{
  public class CreateDoctorCommand : IRequest
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
