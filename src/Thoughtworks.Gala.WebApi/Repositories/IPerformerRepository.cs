using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IPerformerRepository : IRepository<Guid, PerformerEntity>
    {
        Task<IList<PerformerEntity>> GetPerformerEntityListByIdsAsync(Guid[] performerIds,
            CancellationToken cancellationToken) => QueryEntitiesAsync(performerIds, cancellationToken);

        Task<PerformerEntity?> GetPerformerEntityByIdAsync(Guid performerId,
            CancellationToken cancellationToken = default) => ReadEntityAsync(performerId, cancellationToken);
        
        Task<PerformerEntity> CreatePerformerEntityAsync(PerformerEntity performerEntity,
            CancellationToken cancellationToken = default) => CreateEntityAsync(performerEntity, cancellationToken);

        Task<PerformerEntity> UpdatePerformerEntityByIdAsync(Guid performerId, PerformerEntity performerEntity,
            CancellationToken cancellationToken = default) => UpdateEntityAsync(performerId, performerEntity, cancellationToken);

        Task<PerformerEntity> DeletePerformerEntityByIdAsync(Guid performerId,
            CancellationToken cancellationToken = default) => DeleteEntityAsync(performerId, true, cancellationToken);

        Task<PerformerEntity> MarkPerformerEntityAsDeletedAsync(Guid performerId,
            CancellationToken cancellationToken = default) => DeleteEntityAsync(performerId, false, cancellationToken);

        Task<IList<PerformerEntity>> GetPerformerEntityListByGalaIdAsync(Guid galaId, CancellationToken cancellationToken = default);
        
        Task<IList<PerformerEntity>> GetPerformerEntityListByProgramIdAsync(Guid programId, CancellationToken cancellationToken = default);
    }
}