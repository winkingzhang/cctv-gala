using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.ViewModels;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IGalaRepository : IRepository<Guid, GalaEntity>
    {
        Task<GalaEntity?> GetGalaEntityByIdAsync(Guid galaId,
            CancellationToken cancellationToken = default) => ReadEntityAsync(galaId, cancellationToken);

        Task<IList<GalaEntity>> GetGalaEntityListByIdsAsync(Guid[] galaIds,
            CancellationToken cancellationToken = default) => QueryEntitiesAsync(galaIds, cancellationToken);

        Task<IList<GalaEntity>> GetGalaEntityListByYearsAsync(int[] years,
            CancellationToken cancellationToken = default);

        Task<IList<GalaEntity>> GetGalaEntityListByZodiacAsync(ChineseZodiac zodiac,
            CancellationToken cancellationToken = default)
        {
            var searchYears = new List<int>();
            var possibleYear = 1984 + (int) zodiac - 1;
            while (possibleYear < DateTime.Now.Year)
            {
                searchYears.Add(possibleYear);
                possibleYear += 12;
            }

            if (zodiac == ChineseZodiac.Pig)
            {
                searchYears.Insert(0, 1983);
            }

            if (zodiac == ChineseZodiac.Dog)
            {
                searchYears.Insert(0, 1982);
            }

            return GetGalaEntityListByYearsAsync(searchYears.ToArray(), cancellationToken);
        }

        Task<GalaEntity> CreateGalaEntityAsync(GalaEntity galaEntity,
            CancellationToken cancellationToken = default) => CreateEntityAsync(galaEntity, cancellationToken);

        Task<GalaEntity> UpdateGalaEntityByIdAsync(Guid galaId, GalaEntity galaEntity,
            CancellationToken cancellationToken = default) => UpdateEntityAsync(galaId, galaEntity, cancellationToken);

        Task<GalaEntity> DeleteGalaEntityByIdAsync(Guid galaId,
            CancellationToken cancellationToken = default) => DeleteEntityAsync(galaId, true, cancellationToken);

        Task<GalaEntity> MarkGalaEntityAsDeletedAsync(Guid galaId,
            CancellationToken cancellationToken = default) => DeleteEntityAsync(galaId, false, cancellationToken);
    }
}