using System;
using System.Diagnostics.CodeAnalysis;

namespace Thoughtworks.Gala.WebApi.Exceptions
{
    public sealed class NotSupportedException : Exception
    {
        public NotSupportedException([NotNull] string type)
            : base($"object [{type}] was not assignable")
        { }
    }
}
