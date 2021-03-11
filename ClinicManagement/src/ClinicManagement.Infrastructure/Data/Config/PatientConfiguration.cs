using ClinicManagement.Core.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Config
{
  public class PatientConfiguration : IEntityTypeConfiguration<Patient>
  {
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
      builder
        .ToTable("Patients").HasKey(k => k.Id);
      builder
        .OwnsOne(p => p.AnimalType, p =>
        {
          p.Property(pp => pp.Breed).HasColumnName("AnimalType_Breed").HasMaxLength(50);
          p.Property(pp => pp.Species).HasColumnName("AnimalType_Species").HasMaxLength(50);
        });

      builder.Metadata.FindNavigation(nameof(Patient.AnimalType))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
  }
}
