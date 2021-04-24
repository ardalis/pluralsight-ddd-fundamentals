using FrontDesk.Core.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontDesk.Infrastructure.Data.Config
{
  public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
  {
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
      builder.Property(p => p.Id).ValueGeneratedNever();
      builder.Ignore(s => s.DateRange);
    }
  }
}
