using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using Thoughtworks.Gala.WebApi.ModelBinders;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.ModelBinders
{
    public class YearsModelBinderProviderTest
    {
        [Fact]
        public void Should_Get_ArgumentNullException_When_GetBinder_With_ContextNull()
        {
            var provider = new YearsModelBinderProvider();
            Assert.Throws<ArgumentNullException>(() => { _ = provider.GetBinder(null); });
        }

        [Fact]
        public void Should_Get_YearsModelBinder_When_GetBinder_With_IntArray()
        {
            var metaDataMock = new Mock<ModelMetadata>(MockBehavior.Loose,
                ModelMetadataIdentity.ForType(typeof(int[])));
            var contextMock = new Mock<ModelBinderProviderContext>();
            contextMock.SetupGet(ctx => ctx.Metadata)
                .Returns(metaDataMock.Object);

            var provider = new YearsModelBinderProvider();
            var binder = provider.GetBinder(contextMock.Object);
            Assert.NotNull(binder);
            Assert.IsType<YearsModelBinder>(binder);
        }
    }
}