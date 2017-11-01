using System;

namespace BoltJwt.Application.Middlewares.Authentication
{
    public class TokenOptions
    {
        /// <summary>
        /// The relative request path to listen on.
        /// </summary>
        /// <remarks>The default path is <c>/token</c>.</remarks>
        public string Path { get; set; }

        /// <summary>
        /// The expiration time for the generated tokens.
        /// </summary>
        /// <remarks>The default is 1 week.</remarks>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(7);
    }
}