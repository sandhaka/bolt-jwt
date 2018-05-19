using BoltJwt.Domain.Events;
using BoltJwt.Domain.Model.Abstractions;

namespace BoltJwt.Domain.Model.Aggregates.Configuration
{
    /// <summary>
    /// Configuration data
    /// </summary>
    public class Configuration : AggregateRoot
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

        /// <summary>
        /// Update configuration info
        /// </summary>
        /// <param name="dto">Dto info</param>
        public void UpdateInfo(Configuration dto)
        {
            SmtpHostName = dto?.SmtpHostName ?? SmtpHostName;
            SmtpPassword = dto?.SmtpPassword ?? SmtpPassword;
            SmtpPort = dto?.SmtpPort > 0 ? dto.SmtpPort : SmtpPort;
            SmtpUserName = dto?.SmtpUserName ?? SmtpUserName;
            SmtpEmail = dto?.SmtpEmail ?? SmtpEmail;

            EndpointFqdn = dto?.EndpointFqdn ?? EndpointFqdn;
            EndpointPort = dto?.EndpointPort > 0 ? dto.EndpointPort : EndpointPort;

            if (dto?.RootPassword != null && string.CompareOrdinal(RootPassword, dto.RootPassword) != 0)
            {
                // Save as md5 hash
                RootPassword = dto.RootPassword;

                // Root password has changed
                AddDomainEvent(new RootPasswordChangedDomainEvent { Password = dto.RootPassword});
            }
        }
    }
}