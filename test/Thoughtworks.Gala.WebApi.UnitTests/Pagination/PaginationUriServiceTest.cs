using System;
using Thoughtworks.Gala.WebApi.Pagination;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests
{
    public class PaginationUriServiceTest
    {
        [Theory]
        [InlineData(5, 10, "/api/v1/galas", "http://localhost:5000/api/v1/galas?pageNumber=5&pageSize=10")]
        [InlineData(2, 5, "/api/programs", "http://localhost:5000/api/programs?pageNumber=2&pageSize=5")]
        public void Should_GetPageUri_With_DefaultFilter(
            int pageNumber,
            int pageSize,
            string route,
            string expected)
        {
            var filter = new PaginationFilter
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var uriService = new PaginationUriService("http://localhost:5000");

            var actual = uriService.GetPageUri(filter, route);

            Assert.NotNull(actual);
            Assert.Equal(expected, actual.AbsoluteUri);
        }

        [Fact]
        public void Should_GetPageUrl_With_LargePageSize()
        {
            var filter = new PaginationFilter(12, 456);
            var uriService = new PaginationUriService("http://localhost:5000");
            var actual = uriService.GetPageUri(filter, "/api/performers");

            Assert.NotNull(actual);
            Assert.Equal("http://localhost:5000/api/performers?pageNumber=12&pageSize=50", actual.AbsoluteUri);

        }
    }
}
