using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
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

        [ExcludeFromCodeCoverage]
        public Task<IList<PerformerEntity>> GetPerformerEntityListByGalaIdAsync(Guid galaId,
            CancellationToken cancellationToken = default) =>
            QueryPerformerEntityListByIdWithIndexAsync(galaId,
                "GalaId",
                "PerformerByGalaIdIndex",
                cancellationToken);

        [ExcludeFromCodeCoverage]
        public Task<IList<PerformerEntity>> GetPerformerEntityListByProgramIdAsync(Guid programId,
            CancellationToken cancellationToken = default) =>
            QueryPerformerEntityListByIdWithIndexAsync(programId,
                "ProgramId",
                "PerformerByGalaIdIndex",
                cancellationToken);

        [ExcludeFromCodeCoverage]
        private async Task<IList<PerformerEntity>> QueryPerformerEntityListByIdWithIndexAsync(Guid id,
            string attributeName,
            string indexName,
            CancellationToken cancellationToken = default)
        {
            var search = Context.QueryAsync<PerformerEntity>(
                new QueryOperationConfig()
                {
                    Filter = new QueryFilter(attributeName, QueryOperator.Equal, id)
                },
                new DynamoDBOperationConfig {IndexName = indexName});
            return await search.GetRemainingAsync(cancellationToken);
        }
    }
}