using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model
{
    /// <summary>
    /// Configuration data
    /// </summary>
    public class Configuration : Entity
    {
        public Configuration()
        {
            // Force to have only one record of this entity on the db
            Id = 1;
        }

        // Smtp
        public string SmtpHostName { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpEmail { get; set; }
        public string SmtpPassword { get; set; }

        // System
        public string EndpointFqdn { get; set; }
        public int EndpointPort { get; set; }
        public string RootPassword { get; set; }
    }
}