using FrontDesk.Core.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontDesk.Infrastructure.Data.Config
{
  public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
  {
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
      builder.Property(t => t.Id)
          .IsRequired();
    }
  }
}
