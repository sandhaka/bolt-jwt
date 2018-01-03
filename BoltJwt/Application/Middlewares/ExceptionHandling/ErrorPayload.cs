namespace BoltJwt.Application.Middlewares.ExceptionHandling
{
    public class ErrorPayload
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public int StatusCode { get; set; }
        public string StackTrace { get; set; }
    }
}