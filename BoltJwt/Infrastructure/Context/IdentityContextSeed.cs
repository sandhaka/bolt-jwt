using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BoltJwt.Model;
using Microsoft.Extensions.Logging;

namespace BoltJwt.Infrastructure.Context
{
    public static class IdentityContextSeed
    {
        /// <summary>
        /// Initialize the users table with the root user
        /// </summary>
        /// <param name="services"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="retry"></param>
        /// <returns>Task</returns>
        public static async Task SeedAsync(IServiceProvider services, ILoggerFactory loggerFactory, int retry = 0)
        {
            var availabilityRetry = retry;
            try
            {
                var context = (IdentityContext) services.GetService(typeof(IdentityContext));
                if (!context.Users.Any(i=>i.Root && i.UserName == "root"))
                {
                    context.Users.Add(CreateRoot());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var log = loggerFactory.CreateLogger(nameof(IdentityContextSeed));
                log.LogError(e.Message);

                if (availabilityRetry < 10)
                {
                    availabilityRetry++;
                    await SeedAsync(services, loggerFactory, availabilityRetry);
                }
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
                Admin = true,
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