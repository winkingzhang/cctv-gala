using System;

namespace Thoughtworks.Gala.WebApi.Pagination
{
    public interface IPaginationUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
