﻿using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BoltJwt.Application.Middlewares.Authentication;
using BoltJwt.Application.Services;
using BoltJwt.Infrastructure.Context;
using BoltJwt.Infrastructure.Modules;
using BoltJwt.Infrastructure.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
             // Add Db context through dependency injection. As default the context is instantiate by scope.
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

            // Enable Cross origin requests
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
                options.AddCustomPolicies();
            });

            // Adding options
            services.AddOptions();

            // Add Mvc
            services.AddMvc();

            // Using Autofac as additional dependency injection container
            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new ApplicationModule());
            container.RegisterModule(new MediatorModule());

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