using System;
using System.IO;
using BoltJwt.Infrastructure.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BoltJwt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).MigrateDbContext<IdentityContext>((context, services) =>
            {
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("BuildWebHost");

                try
                {
                    IdentityContextSeed.SeedAsync(context, loggerFactory).Wait();
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "An error occurred seeding the DB.");
                }
            }).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}