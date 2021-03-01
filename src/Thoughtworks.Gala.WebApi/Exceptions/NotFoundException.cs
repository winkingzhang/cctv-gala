using System;
using System.Diagnostics.CodeAnalysis;

namespace Thoughtworks.Gala.WebApi.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException([NotNull] string key)
            : base($"object [{key}] was not found")
        { }
    }
}
