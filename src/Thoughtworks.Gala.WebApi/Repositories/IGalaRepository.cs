using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IGalaRepository : IRepository<Guid, GalaEntity>
    {
        Task<GalaEntity> GetGalaEntityByIdAsync(Guid galaId, CancellationToken cancellationToken = default)
        {
            return ReadEntityAsync(galaId, cancellationToken);
        }

        Task<IList<GalaEntity>> GetGalaEntityListByIdsAsync(Guid[] galaIds, CancellationToken cancellationToken = default)
        {
            return QueryEntitiesAsync(galaIds, cancellationToken);
        }

        Task<IList<GalaEntity>> GetGalaEntityListByYearsAsync(int[] years, CancellationToken cancellationToken = default);

        Task<GalaEntity> CreateGalaEntityAsync(GalaEntity galaEntity, CancellationToken cancellationToken = default)
        {
            return CreateEntityAsync(galaEntity, cancellationToken);
        }

        Task<GalaEntity> UpdateGalaEntityByIdAsync(Guid galaId, GalaEntity galaEntity, CancellationToken cancellationToken = default)
        {
            return UpdateEntityAsync(galaId, galaEntity, cancellationToken);
        }

        Task<GalaEntity> DeleteGalaEntityByIdAsync(Guid galaId, CancellationToken cancellationToken = default)
        {
            return DeleteEntityAsync(galaId, true, cancellationToken);
        }

        Task<GalaEntity> MarkGalaEntityAsDeletedAsync(Guid galaId, CancellationToken cancellationToken = default)
        {
            return DeleteEntityAsync(galaId, false, cancellationToken);
        }
    }
}
