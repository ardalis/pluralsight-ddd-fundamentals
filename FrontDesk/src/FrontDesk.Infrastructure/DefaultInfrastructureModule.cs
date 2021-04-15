using System.Collections.Generic;
using System.Reflection;
using Autofac;
using FrontDesk.Core.Interfaces;
using FrontDesk.Infrastructure.Data;
using FrontDesk.Infrastructure.Messaging;
using MediatR;
using PluralsightDdd.SharedKernel.Interfaces;
using Module = Autofac.Module;
using FrontDesk.Core.ScheduleAggregate;

namespace FrontDesk.Infrastructure
{
  public class DefaultInfrastructureModule : Module
  {
    private readonly bool _isDevelopment = false;
    private readonly List<Assembly> _assemblies = new List<Assembly>();

    public DefaultInfrastructureModule(bool isDevelopment, Assembly callingAssembly = null)
    {
      _isDevelopment = isDevelopment;

      var coreAssembly = Assembly.GetAssembly(typeof(Schedule));
      _assemblies.Add(coreAssembly);

      var infrastructureAssembly = Assembly.GetAssembly(typeof(AppDbContext));
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
      builder.RegisterGeneric(typeof(EfRepository<>))
          .As(typeof(IRepository<>))
          .InstancePerLifetimeScope();

      builder.RegisterGeneric(typeof(EfRepository<>))
          .InstancePerLifetimeScope();

      // add a cache
      builder.RegisterGeneric(typeof(CachedRepository<>))
        .As(typeof(IReadRepository<>))
          .InstancePerLifetimeScope();

      builder.RegisterType(typeof(RabbitMessagePublisher))
        .As(typeof(IMessagePublisher))
        .InstancePerLifetimeScope();

// MediatR is registered in FrontDesk.Api
//      builder
//          .RegisterType<Mediator>()
//          .As<IMediator>()
//          .InstancePerLifetimeScope();

//      var mediatrOpenTypes = new[]
//{
//        typeof(IRequestHandler<,>),
//        typeof(IRequestExceptionHandler<,,>),
//        typeof(IRequestExceptionAction<,>),
//        typeof(INotificationHandler<>),
//      };

//      foreach (var mediatrOpenType in mediatrOpenTypes)
//      {
//        builder
//        .RegisterAssemblyTypes(_assemblies.ToArray())
//        .AsClosedTypesOf(mediatrOpenType)
//        .AsImplementedInterfaces();
//      }

      builder.Register<ServiceFactory>(context =>
      {
        var c = context.Resolve<IComponentContext>();
        return t => c.Resolve(t);
      });

      builder.RegisterType<EmailSender>().As<IEmailSender>()
          .InstancePerLifetimeScope();

      builder.RegisterType<AppDbContextSeed>().InstancePerLifetimeScope();
    }

    private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {
      // Add development only services
    }

    private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
      // Add production only services
    }
  }
}
