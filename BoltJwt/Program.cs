using System;
using System.IO;
using System.Net;
using BoltJwt.Infrastructure.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BoltJwt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().MigrateDbContext<IdentityContext>((context, services) =>
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

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 25;
                    options.Limits.MaxConcurrentUpgradedConnections = 25;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Any, 443, listenOptions =>
                        {
                            listenOptions.UseHttps("certs/dev.boltjwt.pfx",
                                File.ReadAllText("certs/dev.boltjwt.passphrase"));
                        });
                });
    }
}