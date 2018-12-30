using System;
using System.IO;

namespace BoltJwt.Infrastructure.Security
{
    public static class CertsProviderService
    {
        public static string GetCertificatePath()
        {
            string certName;

            if (File.Exists("/app/secrets/auth/cert_pfx"))
            {
                certName = "/app/secrets/auth/cert_pfx";
            }
            else
            {
                certName = Environment.GetEnvironmentVariable("CERT_NAME");
            }

            return certName;
        }

        public static string GetCertificatePassphrase()
        {
            string certPwdName;

            if (File.Exists("/app/secrets/auth/passphrase"))
            {
                certPwdName = "/app/secrets/auth/passphrase";
            }
            else
            {
                certPwdName = Environment.GetEnvironmentVariable("CERT_PWD_NAME");
            }

            return certPwdName;
        }
    }
}