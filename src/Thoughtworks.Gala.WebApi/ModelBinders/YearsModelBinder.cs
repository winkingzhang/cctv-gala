using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Thoughtworks.Gala.WebApi.ModelBinders
{
    public sealed class YearsModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext? context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            return context.Metadata.ModelType == typeof(int[]) ? new YearsModelBinder() : default;
        }
    }

    public class YearsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var years = bindingContext.ValueProvider.GetValue("years").FirstValue;
            if (string.IsNullOrEmpty(years))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            
            bindingContext.Result = ModelBindingResult.Success(
                years.Split(",")
                    .Select(sy => sy.Trim())
                    .Select(sy => int.TryParse(sy, out var y) ? y : -1)
                    .Where(y => y > 0)
                    .ToArray());
            return Task.CompletedTask;
        }
    }
}