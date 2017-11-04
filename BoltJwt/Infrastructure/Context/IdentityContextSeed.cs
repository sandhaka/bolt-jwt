using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BoltJwt.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoltJwt.Infrastructure.Context
{
    public static class IdentityContextSeed
    {
        /// <summary>
        /// Initialize the users table with the root user
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
                        context.Authorizations.Add(new DefinedAuthorization
                        {
                            Name = Constants.AdministrativeAuth
                        });

                        await context.SaveChangesAsync();
                    }

                    if (!context.Users.Any(i => i.Root && i.UserName == "root"))
                    {
                        context.Users.Add(CreateRoot());

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
            var md5Hascher = MD5.Create();
            var passwordHash = md5Hascher.ComputeHash(Encoding.UTF8.GetBytes("root"));
            var sBuilder = new StringBuilder();

            foreach (var t in passwordHash)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            var user = new User()
            {
                Root = true,
                Name = "Root",
                Surname = "Root",
                UserName = "root",
                Email = "root@system.local",
                Password = sBuilder.ToString()
            };

            return user;
        }
    }
}