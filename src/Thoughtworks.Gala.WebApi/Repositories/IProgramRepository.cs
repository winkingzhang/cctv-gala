using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IProgramRepository : IRepository<Guid, ProgramEntity>
    {
        Task<IList<ProgramEntity>> GetProgramEntityListByIdsAsync(Guid[] programIds,
            CancellationToken cancellationToken) => QueryEntitiesAsync(programIds, cancellationToken);

        Task<ProgramEntity?> GetProgramEntityByIdAsync(Guid programId,
            CancellationToken cancellationToken = default) => ReadEntityAsync(programId, cancellationToken);
        
        Task<ProgramEntity> CreateProgramEntityAsync(ProgramEntity programEntity,
            CancellationToken cancellationToken = default) => CreateEntityAsync(programEntity, cancellationToken);

        Task<ProgramEntity> UpdateProgramEntityByIdAsync(Guid programId, ProgramEntity programEntity,
            CancellationToken cancellationToken = default) => UpdateEntityAsync(programId, programEntity, cancellationToken);

        Task<ProgramEntity> DeleteProgramEntityByIdAsync(Guid programId,
            CancellationToken cancellationToken = default) => DeleteEntityAsync(programId, true, cancellationToken);

        Task<ProgramEntity> MarkProgramEntityAsDeletedAsync(Guid programId,
            CancellationToken cancellationToken = default) => DeleteEntityAsync(programId, false, cancellationToken);

        // Task<IList<ProgramEntity>> GetProgramEntityListByGalaIdAsync(Guid galaId, CancellationToken cancellationToken = default);
    }
}