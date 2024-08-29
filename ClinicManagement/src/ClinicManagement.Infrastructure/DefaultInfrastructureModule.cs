using ClinicManagement.Core.Interfaces;
using ClinicManagement.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Infrastructure
{
  public static class DefaultInfrastructureModule
  {
    public static void AddInfrastructureDependencies(this IServiceCollection services, bool isDevelopment)
    {
      if (isDevelopment)
      {
        RegisterDevelopmentOnlyDependencies(services);
      }
      else
      {
        RegisterProductionOnlyDependencies(services);
      }

      RegisterCommonDependencies(services);
    }

    private static void RegisterCommonDependencies(IServiceCollection services)
    {
      services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

      services.AddScoped<IEmailSender, EmailSender>();

      services.AddScoped<AppDbContextSeed>();
    }

    private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services)
    {
      // TODO: Add development only services
    }

    private static void RegisterProductionOnlyDependencies(IServiceCollection services)
    {
      // TODO: Add production only services
    }
  }
}
