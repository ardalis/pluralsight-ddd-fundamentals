using System;
using System.Collections.Generic;
using System.Linq;
using BlazorShared;
using FrontDesk.Api.Hubs;
using FrontDesk.Core.Interfaces;
using FrontDesk.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PluralsightDdd.SharedKernel.Interfaces;

namespace FrontDesk.Api
{
  public class Startup
  {
    public const string CORS_POLICY = "CorsPolicy";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
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


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddScoped(typeof(IRepository), typeof(EfRepository));

      services.AddSignalR();

      services.AddMemoryCache();

      var baseUrlConfig = new BaseUrlConfiguration();
      Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);

      services.AddCors(options =>
      {
        options.AddPolicy(name: CORS_POLICY,
                                builder =>
                                {
                              builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'));
                              builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
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

      // Wire up application settings
      services.AddSingleton(typeof(IApplicationSettings), typeof(OfficeSettings));

      services.AddAutoMapper(typeof(Startup).Assembly);
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
          });
      });

      //services.AddHostedService<RabbitMQService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseResponseCompression();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(CORS_POLICY);

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<ScheduleHub>("/schedulehub");
      });
    }
  }
}
