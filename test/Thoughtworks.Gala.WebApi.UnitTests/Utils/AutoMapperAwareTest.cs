using AutoMapper;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.Utils
{
    public abstract class AutoMapperAwareTest : IClassFixture<AutoMapperFixture>
    {
        protected IMapper Mapper { get; }

        protected AutoMapperAwareTest(AutoMapperFixture fixture)
        {
            Mapper = fixture.Mapper;
        }
    }
}