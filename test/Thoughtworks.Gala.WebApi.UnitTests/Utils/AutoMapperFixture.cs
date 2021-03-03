using System;
using AutoMapper;
using Thoughtworks.Gala.WebApi.Mappers;

namespace Thoughtworks.Gala.WebApi.UnitTests.Utils
{
    public sealed class AutoMapperFixture : IDisposable
    {
        public IMapper Mapper { get; } = CreateMapper();

        private static IMapper CreateMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GalaProfiler());
                mc.AddProfile(new ProgramProfiler());
                mc.AddProfile(new PerformerProfiler());
            });
            return mappingConfig.CreateMapper();
        }

        public void Dispose()
        {
            //nothing here
        }
    }
}