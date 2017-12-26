namespace BoltJwt.Controllers.Dto
{
    public class ConfigurationDto
    {
        public string SmtpHostName { get; set; }
        public ushort SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
    }
}