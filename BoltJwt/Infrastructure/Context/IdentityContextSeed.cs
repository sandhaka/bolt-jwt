using System;
using System.Linq;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using BoltJwt.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoltJwt.Infrastructure.Context
{
    public static class IdentityContextSeed
    {
        /// <summary>
        /// Initialize the db
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        /// <returns>Task</returns>
        public static async Task SeedAsync(IdentityContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                using (context)
                {
                    context.Database.Migrate();

                    if (!context.Authorizations.Any(i => i.Name.Equals(Constants.AdministrativeAuth)))
                    {
                        context.Authorizations.Add(new DefinedAuthorization(Constants.AdministrativeAuth));

                        await context.SaveChangesAsync();
                    }

                    if (!context.Users.Any(i => i.Root && i.UserName == "root"))
                    {
                        context.Users.Add(CreateRoot());

                        await context.SaveChangesAsync();
                    }

                    if (!context.Configuration.Any())
                    {
                        context.Configuration.Add(CreateDefaultConfig());

                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                var log = loggerFactory.CreateLogger(nameof(IdentityContextSeed));
                log.LogError(e.Message);
            }
        }

        private static User CreateRoot()
        {
            var user = new User()
            {
                Root = true,
                Name = "Root",
                Surname = "Root",
                UserName = "root",
                Email = "root@system.local",
                Password = "root".ToMd5Hash(),
                Disabled = false
            };

            return user;
        }

        private static Configuration CreateDefaultConfig()
        {
            return new Configuration
            {
                SmtpHostName = "localhost",
                SmtpUserName = "root",
                SmtpEmail = "root@boltjwt.local",
                SmtpPort = 25,
                SmtpPassword = "myPassword",
                EndpointFqdn = "localhost",
                EndpointPort = 80,
                RootPassword = "root".ToMd5Hash()
            };
        }
    }
}