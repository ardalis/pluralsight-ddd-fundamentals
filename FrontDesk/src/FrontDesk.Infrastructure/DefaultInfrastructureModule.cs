using System.Collections.Generic;
using System.Reflection;
using Autofac;
using FrontDesk.Core;
using FrontDesk.Core.Interfaces;
using FrontDesk.Infrastructure.Data;
using MediatR;
using MediatR.Pipeline;
using PluralsightDdd.SharedKernel.Interfaces;
using Module = Autofac.Module;

namespace FrontDesk.Infrastructure
{
  public class DefaultInfrastructureModule : Module
  {
    private readonly bool _isDevelopment = false;
    private readonly List<Assembly> _assemblies = new List<Assembly>();

    public DefaultInfrastructureModule(bool isDevelopment, Assembly callingAssembly = null)
    {
      _isDevelopment = isDevelopment;
      var coreAssembly = Assembly.GetAssembly(typeof(DatabasePopulator));
      var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
      _assemblies.Add(coreAssembly);
      _assemblies.Add(infrastructureAssembly);
      if (callingAssembly != null)
      {
        _assemblies.Add(callingAssembly);
      }
    }

    protected override void Load(ContainerBuilder builder)
    {
      if (_isDevelopment)
      {
        RegisterDevelopmentOnlyDependencies(builder);
      }
      else
      {
        RegisterProductionOnlyDependencies(builder);
      }
      RegisterCommonDependencies(builder);
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
      builder.RegisterType<EfRepository>().As<IRepository>()
          .InstancePerLifetimeScope();

      builder
          .RegisterType<Mediator>()
          .As<IMediator>()
          .InstancePerLifetimeScope();

      builder.Register<ServiceFactory>(context =>
      {
        var c = context.Resolve<IComponentContext>();
        return t => c.Resolve(t);
      });

      var mediatrOpenTypes = new[]
      {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

      foreach (var mediatrOpenType in mediatrOpenTypes)
      {
        builder
        .RegisterAssemblyTypes(_assemblies.ToArray())
        .AsClosedTypesOf(mediatrOpenType)
        .AsImplementedInterfaces();
      }

      builder.RegisterType<EmailSender>().As<IEmailSender>()
          .InstancePerLifetimeScope();

      builder.RegisterType<AppDbContextSeed>().InstancePerLifetimeScope();
    }

    private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {
      // TODO: Add development only services
    }

    private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
      // TODO: Add production only services
    }

  }
}
