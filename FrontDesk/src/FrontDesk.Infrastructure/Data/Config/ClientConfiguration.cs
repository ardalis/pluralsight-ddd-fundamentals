using FrontDesk.Core.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontDesk.Infrastructure.Data.Config
{
  public class ClientConfiguration : IEntityTypeConfiguration<Client>
  {
    public void Configure(EntityTypeBuilder<Client> builder)
    {
      builder.ToTable("Clients").HasKey(x => x.Id);
    }
  }
}
