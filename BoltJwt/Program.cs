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
                .UseKestrel(options =>
                {
                    options.Limits.MaxConcurrentConnections = 25;
                    options.Limits.MaxConcurrentUpgradedConnections = 25;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Loopback, 5000, listenOptions =>
                        {
                            listenOptions.UseHttps("certs/dev.boltjwt.pfx",
                                File.ReadAllText("certs/dev.boltjwt.passphrase"));
                        });
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}