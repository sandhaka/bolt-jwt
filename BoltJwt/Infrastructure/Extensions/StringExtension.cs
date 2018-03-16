using System.Security.Cryptography;
using System.Text;

namespace BoltJwt.Infrastructure.Extensions
{
    /// <summary>
    /// String extension class TODO: Resolve model contamination from this class!
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Compute MD5 hash
        /// </summary>
        /// <param name="sz">string</param>
        /// <returns>hash string</returns>
        public static string ToMd5Hash(this string sz)
        {
            using (var md5Hash = MD5.Create())
            {
                var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sz));

                var sBuilder = new StringBuilder();

                foreach (var t in hash)
                {
                    sBuilder.Append(t.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}