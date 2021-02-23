using System;

namespace Thoughtworks.Gala.WebApi.ValueObjects
{
    public class PagedResponse<TViewModel> : Response<TViewModel>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResponse(TViewModel data, int pageNumber, int pageSize) : base(data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
