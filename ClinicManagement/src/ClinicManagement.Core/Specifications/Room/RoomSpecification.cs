using Ardalis.Specification;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Core.Specifications
{
  public class RoomSpecification : Specification<Room>
  {
    public RoomSpecification()
    {
      Query.OrderBy(room => room.Name);
    }
  }
}
