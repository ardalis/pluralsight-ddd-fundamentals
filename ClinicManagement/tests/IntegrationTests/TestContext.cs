using ClinicManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests
{
  /// <summary>
  /// See: https://stackoverflow.com/a/60497822
  /// </summary>
  internal sealed class TestContext : AppDbContext
  {
    public TestContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options, mediator)
    {
      Database.OpenConnection();
      Database.EnsureCreated();
    }

    public override void Dispose()
    {
      Database.CloseConnection();
      base.Dispose();
    }
  }
}
