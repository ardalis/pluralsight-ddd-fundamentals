using FrontDesk.Core.Interfaces;
using FrontDesk.Infrastructure.Data;
using FrontDesk.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Infrastructure
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

      services.AddScoped(typeof(EfRepository<>));

      // add a cache
      services.AddScoped(typeof(IReadRepository<>), typeof(CachedRepository<>));

      services.AddScoped<IEmailSender, EmailSender>();

      services.AddScoped<AppDbContextSeed>();
    }

    private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services)
    {
      // Add development only services
    }

    private static void RegisterProductionOnlyDependencies(IServiceCollection services)
    {
      // Add production only services
    }
  }
}
