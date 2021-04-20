using System;
using Thoughtworks.Gala.WebApi.Entities;

namespace Thoughtworks.Gala.WebApi.Repositories
{
    public interface IProgramRepository : IRepository<Guid, ProgramEntity>
    {
    }
}