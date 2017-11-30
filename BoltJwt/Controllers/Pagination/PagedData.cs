using System.Collections.Generic;

namespace BoltJwt.Controllers.Pagination
{
    /// <summary>
    /// Pagination dto
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Data { get; set; }
        public PageQuery Page { get; set; }
    }
}