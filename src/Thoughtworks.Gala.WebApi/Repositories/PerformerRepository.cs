using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public sealed class PerformerRepository : Repository<Guid, PerformerEntity>, IPerformerRepository
    {
        public PerformerRepository([NotNull] IDynamoDBContext context)
            : base(context)
        {
        }

        protected override Guid NextKey()
        {
            return Guid.NewGuid();
        }
    }
}
