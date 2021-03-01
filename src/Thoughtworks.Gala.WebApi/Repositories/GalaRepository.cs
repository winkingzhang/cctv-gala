using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public sealed class GalaRepository : Repository<Guid, GalaEntity>
    {
        public GalaRepository([NotNull] IDynamoDBContext context)
            : base(context)
        {
        }

        protected override Guid NextKey()
        {
            return Guid.NewGuid();
        }
    }
}
