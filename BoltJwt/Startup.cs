using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BoltJwt.Application.Middlewares.Authentication;
using BoltJwt.Application.Services;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Infrastructure.Modules;
using BoltJwt.Infrastructure.Security;
using BoltJwt.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BoltJwt
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"settings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add the db context
            var connectionString = Configuration.GetConnectionString("mssql.data.connection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new FormatException("The connection string is empty");
            }

            services.AddEntityFrameworkSqlServer().AddDbContext<IdentityContext>(options =>
                {
                    options.UseSqlServer(connectionString, opts =>
                    {
                        // Give the name of the assembly that contain the migration instructions (the 'migrations' folder)
                        opts.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        opts.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                });

            // Add cors and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials() );
            });

            // Add Jwt based authentication
            services.AddJwtBearerAuthentication(Configuration);

            // Add authorization policies
            services.AddAuthorization(options =>
            {
                // Adding a custom policy to control the access to the controllers.
                // Users, groups and authorizations can be edited only by the root or by a 'admin' user.
                options.AddPolicy("BoltJwtAdmin", policyBuilder => policyBuilder.AddRequirements(
                    new AuthorizationsRequirement(
                        Constants.AdministrativeAuth,
                        Constants.RootAuth))
                );
            });

            // Add Mvc service and setup the base authorization policy globally
            services.AddMvc(config =>
            {
                // Require authenticated user as default
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddOptions();

            // Di
            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider services)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Trace);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Warning);
            }

            // Configuring the Json Web Token provider
            app.UseJwtProvider(services, Configuration);

            // Mvc
            app.UseMvc();
        }
    }
}