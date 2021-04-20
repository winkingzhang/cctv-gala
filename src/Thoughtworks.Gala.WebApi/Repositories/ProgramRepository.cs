using Amazon.DynamoDBv2.DataModel;
using System;
using System.Diagnostics.CodeAnalysis;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public sealed class ProgramRepository : Repository<Guid, ProgramEntity>, IProgramRepository
    {
        public ProgramRepository([NotNull] IDynamoDBContext context)
            : base(context)
        {
        }

        protected override Guid NextKey()
        {
            return Guid.NewGuid();
        }

        // public Task<IList<ProgramEntity>> GetProgramEntityListByGalaIdAsync(Guid galaId, CancellationToken cancellationToken = default)
        // {
        //     var batchGet = Context.CreateBatchGet<ProgramEntity>(new DynamoDBOperationConfig { IndexName = "GalaIdIndex" });
        //     years.ToList().ForEach(key => batchGet.AddKey(key));
        //     await batchGet.ExecuteAsync(cancellationToken);
        //     return batchGet.Results;
        // }
    }
}
