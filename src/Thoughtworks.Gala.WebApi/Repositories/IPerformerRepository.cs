using System;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IPerformerRepository : IRepository<Guid, PerformerEntity>
    {
    }
}