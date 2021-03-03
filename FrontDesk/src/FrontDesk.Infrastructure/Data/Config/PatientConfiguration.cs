using FrontDesk.Core.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontDesk.Infrastructure.Data.Config
{
  public class PatientConfiguration : IEntityTypeConfiguration<Patient>
  {
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
      builder.ToTable("Patients").HasKey(x => x.Id);
      builder.OwnsOne(p => p.AnimalType, p =>
      {
        p.Property(pp => pp.Breed).HasColumnName("AnimalType_Breed").HasMaxLength(ColumnConstants.DEFAULT_NAME_LENGTH);
        p.Property(pp => pp.Species).HasColumnName("AnimalType_Species").HasMaxLength(ColumnConstants.DEFAULT_NAME_LENGTH);
      });
    }
  }
}
