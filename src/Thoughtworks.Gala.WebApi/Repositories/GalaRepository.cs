using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public sealed class GalaRepository : Repository<Guid, GalaEntity>, IGalaRepository
    {
        public GalaRepository([NotNull] IDynamoDBContext context)
            : base(context)
        {
        }

        protected override Guid NextKey()
        {
            return Guid.NewGuid();
        }

        public Task<IList<GalaEntity>> GetGalaEntityListByYearsAsync(int[] years, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
