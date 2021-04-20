using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Thoughtworks.Gala.WebApi.ValueObjects
{
    public abstract class ErrorResponse : Response<dynamic>
    {
        private ErrorResponse(HttpStatusCode code, IDictionary<string, dynamic>? errors) : base()
        {
            Message = code.ToString();
            errors?.ToList().ForEach(e => Errors?.Add(e.Key, e.Value));
        }

        public class BadRequest : ErrorResponse
        {
            public BadRequest(IDictionary<string, dynamic>? errors)
                : base(HttpStatusCode.BadRequest, errors)
            {
            }

            public BadRequest(ModelStateDictionary? modelState)
                : this(modelState is null ? null : FromModelState(modelState!))
            {
            }

            private static IDictionary<string, dynamic> FromModelState(ModelStateDictionary modelState)
            {
                return modelState.ToDictionary<KeyValuePair<string, ModelStateEntry>, string, dynamic>(
                    ms => ms.Key,
                    ms => ms.Value.ToString() ?? string.Empty);
            }
        }
    }
}