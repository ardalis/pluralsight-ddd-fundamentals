using FrontDesk.Core.ScheduleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontDesk.Infrastructure.Data.Config
{
  public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
  {
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
      builder.ToTable("Appointments").HasKey(x => x.Id);
      builder.Property(p => p.Id).ValueGeneratedNever();
      builder.OwnsOne(p => p.TimeRange, p =>
      {
        p.Property(pp => pp.Start)
        .HasColumnName("TimeRange_Start");
        p.Property(pp => pp.End)
        .HasColumnName("TimeRange_End");
      });
      builder.Property(p => p.Title)
        .HasMaxLength(ColumnConstants.DEFAULT_NAME_LENGTH);
    }
  }
}
