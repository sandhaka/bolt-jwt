using System.Collections.Generic;
using BoltJwt.Controllers.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BoltJwt.Controllers.Pagination
{
    public class PageQuery
    {
        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }
        [FromQuery(Name = "size")]
        public int Size { get; set; }
        [FromQuery(Name = "totalElements")]
        public int TotalElements { get; set; }
        [FromQuery(Name = "totalPages")]
        public int TotalPages { get; set; }
        [FromQuery(Name = "filters")]
        public string Filters { get; set; }
    }
}
