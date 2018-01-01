namespace BoltJwt.Controllers.Dto
{
    public class ConfigurationDto
    {
        public string SmtpHostName { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }

        public string EndpointFqdn { get; set; }
        public int EndpointPort { get; set; }
    }
}