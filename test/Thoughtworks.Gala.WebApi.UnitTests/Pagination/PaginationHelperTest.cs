using Thoughtworks.Gala.WebApi.Pagination;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.Pagination
{
    public class PaginationHelperTest
    {
        private static readonly PaginationUriService UriService
            = new PaginationUriService("http://localhost:5000/");

        [Fact]
        public void Get_PagedResponse_When_Input_Valid()
        {
            var data = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            var pagedResponse = data.ToPagedReponse(
                new PaginationFilter(2, 5),
                data.Length,
                UriService,
                "api/data");

            Assert.NotNull(pagedResponse);

            Assert.NotNull(pagedResponse.Data);
            Assert.Equal(data, pagedResponse.Data);
            
            Assert.True(pagedResponse.Succeeded);
            Assert.Null(pagedResponse.Errors);
            Assert.Empty(pagedResponse.Message);

            Assert.Equal(2, pagedResponse.PageNumber);
            Assert.Equal(5, pagedResponse.PageSize);
            Assert.Equal(3, pagedResponse.TotalPages);
            Assert.Equal(data.Length, pagedResponse.TotalRecords);
            Assert.Equal(
                "http://localhost:5000/api/data?pageNumber=1&pageSize=5",
                pagedResponse.FirstPage.AbsoluteUri
            );
            Assert.Equal(
                "http://localhost:5000/api/data?pageNumber=3&pageSize=5",
                pagedResponse.LastPage.AbsoluteUri
            );
            Assert.Equal(
                "http://localhost:5000/api/data?pageNumber=3&pageSize=5",
                pagedResponse.NextPage.AbsoluteUri
            );
            Assert.Equal(
                "http://localhost:5000/api/data?pageNumber=1&pageSize=5",
                pagedResponse.PreviousPage.AbsoluteUri
            );
        }
    }
}