using System;

namespace BoltJwt.Infrastructure.AppConfigurations
{
    /// <summary>
    /// Configuration data
    /// </summary>
    public class Configuration
    {
        public Configuration()
        {
            // Force to have only one record of this entity on the db
            Id = 1;
        }

        public int Id { get; set; }
        public string SmtpHostName { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
    }
}