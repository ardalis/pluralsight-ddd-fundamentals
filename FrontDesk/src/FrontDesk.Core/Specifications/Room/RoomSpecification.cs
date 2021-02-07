using Ardalis.Specification;
using FrontDesk.Core.Aggregates;

namespace FrontDesk.Core.Specifications
{
  public class RoomSpecification : Specification<Room>
  {
    public RoomSpecification()
    {
      Query.OrderBy(room => room.Name);
    }
  }
}
