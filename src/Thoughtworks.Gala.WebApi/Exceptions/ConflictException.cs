using System;
using System.Diagnostics.CodeAnalysis;

namespace Thoughtworks.Gala.WebApi.Exceptions
{
    public sealed class ConflictException : Exception
    {
        public ConflictException([NotNull] string type)
            : base($"object [{type}] was already existed")
        { }
    }
}
