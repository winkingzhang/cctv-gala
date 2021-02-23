using System;
using System.Collections.Generic;

namespace Thoughtworks.Gala.WebApi.ValueObjects
{
    public class Response<TViewModel>
    {
        protected Response()
        {
            Succeeded = false;
            Errors = new Dictionary<string, dynamic>();
        }

        public Response(TViewModel data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }

        public TViewModel Data { get; set; }
        public bool Succeeded { get; private set; }
        public Dictionary<string, dynamic> Errors { get; set; }
        public string Message { get; set; }
    }
}
