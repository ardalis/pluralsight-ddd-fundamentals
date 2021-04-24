using Ardalis.Specification;
using FrontDesk.Core.SyncedAggregates;

namespace FrontDesk.Core.SyncedAggregates.Specifications
{
  public class RoomSpecification : Specification<Room>
  {
    public RoomSpecification()
    {
      Query.OrderBy(room => room.Name);
    }
  }
}
