using System;
using System.Linq;
using System.Reflection;
using Autofac;
using BlazorShared;
using ClinicManagement.Api.Hubs;
using ClinicManagement.Core.Interfaces;
using ClinicManagement.Infrastructure;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Messaging;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ClinicManagement.Api
{
  public class Startup
  {
    public const string CORS_POLICY = "CorsPolicy";
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      _env = env;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureDevelopmentServices(IServiceCollection services)
    {
      // use in-memory database
      //ConfigureInMemoryDatabases(services);

      // use real database
      ConfigureProductionServices(services);
    }

    public void ConfigureDockerServices(IServiceCollection services)
    {
      ConfigureDevelopmentServices(services);
    }

    private void ConfigureInMemoryDatabases(IServiceCollection services)
    {
      services.AddDbContext<AppDbContext>(c =>
          c.UseInMemoryDatabase("AppDb"));

      ConfigureServices(services);
    }

    public void ConfigureProductionServices(IServiceCollection services)
    {
      // use real database
      // Requires LocalDB which can be installed with SQL Server Express 2016
      // https://www.microsoft.com/en-us/download/details.aspx?id=54284
      services.AddDbContext<AppDbContext>(c =>
          c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      ConfigureServices(services);
    }

    public void ConfigureTestingServices(IServiceCollection services)
    {
      ConfigureInMemoryDatabases(services);
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSignalR();
      services.AddMemoryCache();

      services.AddSingleton(typeof(IApplicationSettings), typeof(OfficeSettings));

      var baseUrlConfig = new BaseUrlConfiguration();
      Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);

      services.AddCors(options =>
      {
        options.AddPolicy(name: CORS_POLICY,
                                builder =>
                                {
                              builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'), "localhost:6100", "localhost:6150");
                              builder.SetIsOriginAllowed(origin => true);
                              builder.AllowAnyMethod();
                              builder.AllowAnyHeader();
                            });
      });

      services.AddControllers();
      services.AddMediatR(typeof(Startup).Assembly);

      services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                  new[] { "application/octet-stream" });
      });

      services.AddAutoMapper(typeof(Startup).Assembly);
      services.AddSwaggerGenCustom();

      // configure messaging
      var messagingConfig = Configuration.GetSection("RabbitMq");
      services.Configure<RabbitMqConfiguration>(messagingConfig);
      services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
      services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      bool isDevelopment = (_env.EnvironmentName == "Development");
      builder.RegisterModule(new DefaultInfrastructureModule(isDevelopment, Assembly.GetExecutingAssembly()));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseResponseCompression();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      //  if enabled configure docker with
      // https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-5.0
      //app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(CORS_POLICY);

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI to app root
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        //endpoints.MapHub<ClinicManagementHub>($"/{SignalRConstants.HUB_NAME}");
      });
    }
  }
}
