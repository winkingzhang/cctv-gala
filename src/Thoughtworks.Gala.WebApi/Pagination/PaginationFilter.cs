namespace Thoughtworks.Gala.WebApi.Pagination
{
    public sealed class PaginationFilter
    {
        const int MaxPageSize = 50;

        private int _pageSize = 10;

        public PaginationFilter() { }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
