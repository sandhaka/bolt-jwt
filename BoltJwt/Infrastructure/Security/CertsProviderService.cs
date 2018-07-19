using System;
using System.IO;

namespace BoltJwt.Infrastructure.Security
{
    public static class CertsProviderService
    {
        public static string GetCertificatePath()
        {
            if (string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production"))
            {
                if (File.Exists("/run/secrets/cert"))
                {
                    return "/run/secrets/cert";
                }
                throw new Exception("Missing file: /run/secrets/cert");
            }

            var certPathEnv = Environment.GetEnvironmentVariable("CERT_NAME");

            if (File.Exists(certPathEnv))
            {
                return certPathEnv;
            }
            throw new Exception($"File not exists: {certPathEnv}");
        }

        public static string GetCertificatePassphrase()
        {
            if (string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production"))
            {
                if (File.Exists("/run/secrets/cert_pwd"))
                {
                    return "/run/secrets/cert_pwd";
                }
                throw new Exception("Missing file: /run/secrets/cert_pwd");
            }

            var certPwdPathEnv = Environment.GetEnvironmentVariable("CERT_PWD_NAME");

            if (File.Exists(certPwdPathEnv))
            {
                return File.ReadAllText(certPwdPathEnv);
            }
            throw new Exception($"File not exists: {certPwdPathEnv}");
        }
    }
}