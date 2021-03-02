﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Diagnostics.CodeAnalysis;
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
