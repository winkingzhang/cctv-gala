using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Moq;
using Thoughtworks.Gala.WebApi.ModelBinders;
using Xunit;

namespace Thoughtworks.Gala.WebApi.UnitTests.ModelBinders
{
    public class YearsModelBinderTest
    {
        [Fact]
        public async Task Should_Get_ModelBindingResult_Failed_When_No_Years()
        {
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(provider => provider.GetValue(It.IsAny<string>()))
                .Returns(new ValueProviderResult(new StringValues(new string[0])));
            var contextMock = new Mock<ModelBindingContext>();
            contextMock.SetupGet(ctx => ctx.ValueProvider)
                .Returns(valueProviderMock.Object);
            contextMock.SetupSet(ctx =>
                    ctx.Result = ModelBindingResult.Failed())
                .Verifiable();

            var binder = new YearsModelBinder();
            await binder.BindModelAsync(contextMock.Object);
            contextMock.Verify();
        }

        [Fact]
        public async Task Should_Get_ModelBindingResult_Success_When_ValidYears()
        {
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(provider => provider.GetValue(It.IsAny<string>()))
                .Returns(new ValueProviderResult(new StringValues(new[] {"1985,2000"})));
            var contextMock = new Mock<ModelBindingContext>();
            contextMock.SetupGet(ctx => ctx.ValueProvider)
                .Returns(valueProviderMock.Object);
            contextMock.SetupSet(ctx =>
                    ctx.Result = It.Is<ModelBindingResult>(r => r.IsModelSet && r.Model != null))
                .Verifiable();

            var binder = new YearsModelBinder();
            await binder.BindModelAsync(contextMock.Object);
            contextMock.Verify();
        }
    }
}