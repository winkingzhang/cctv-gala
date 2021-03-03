using System.Linq;
using AutoMapper;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.ViewModels;

namespace Thoughtworks.Gala.WebApi.Mappers
{
    public sealed class GalaProfiler : Profile
    {
        public GalaProfiler()
        {
            SetupGalaViewModelToGalaEntity();
            SetupGalaViewModelCreationToGalaEntity();
            SetupGalaViewModelEditToGalaEntity();

            SetupGalaEntityToGalaViewModel();
        }

        private void SetupGalaViewModelToGalaEntity()
        {
            CreateMap<GalaViewModel, GalaEntity>()
                .ForMember(
                    e => e.Id,
                    c => c.MapFrom(vm => vm.GalaId)
                )
                .ForMember(
                    e => e.ProgramIds,
                    c => c.MapFrom(vm => vm.ProgramIds.ToList().AsReadOnly())
                );
        }

        private void SetupGalaEntityToGalaViewModel()
        {
            CreateMap<GalaEntity, GalaViewModel>()
                .ForMember(
                    vm => vm.GalaId,
                    c => c.MapFrom(e => e.Id)
                )
                .ForMember(
                    vm => vm.ProgramIds,
                    c => c.MapFrom(e => e.ProgramIds.ToList().AsReadOnly())
                );
        }

        private void SetupGalaViewModelCreationToGalaEntity()
        {
            CreateMap<GalaViewModel.Creation, GalaEntity>()
                .ForMember(
                    e => e.ProgramIds,
                    c => c.MapFrom(vm => vm.ProgramIds.ToList().AsReadOnly())
                );
        }

        private void SetupGalaViewModelEditToGalaEntity()
        {
            CreateMap<GalaViewModel.Edit, GalaEntity>()
                .ForMember(
                    e => e.Id,
                    c => c.MapFrom(vm => vm.GalaId)
                )
                .ForMember(
                    e => e.ProgramIds,
                    c => c.MapFrom(vm => vm.ProgramIds.ToList().AsReadOnly())
                );
        }
    }
}