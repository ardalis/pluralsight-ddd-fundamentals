using FrontDesk.Core.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontDesk.Infrastructure.Data.Config
{
  public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
  {
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
      builder.ToTable("Doctors").HasKey(x => x.Id);
    }
  }
}
