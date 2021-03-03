using AutoMapper;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.ViewModels;

namespace Thoughtworks.Gala.WebApi.Mappers
{
    public sealed class ProgramProfiler : Profile
    {
        public ProgramProfiler()
        {
            SetupProgramViewModelToProgramEntity();
            SetupProgramViewModelCreationToProgramEntity();
            SetupProgramViewModelEditToProgramEntity();

            SetupProgramEntityToProgramViewModel();
        }

        private void SetupProgramViewModelToProgramEntity()
        {
            CreateMap<ProgramViewModel, ProgramEntity>()
                .ForMember(
                    e => e.Id,
                    c => c.MapFrom(vm => vm.ProgramId)
                );
        }

        private void SetupProgramEntityToProgramViewModel()
        {
            CreateMap<ProgramEntity, ProgramViewModel>()
                .ForMember(
                    vm => vm.ProgramId,
                    c => c.MapFrom(e => e.Id)
                );
        }

        private void SetupProgramViewModelCreationToProgramEntity()
        {
            CreateMap<ProgramViewModel.Creation, ProgramEntity>();
        }

        private void SetupProgramViewModelEditToProgramEntity()
        {
            CreateMap<ProgramViewModel.Edit, ProgramEntity>()
                .ForMember(
                    e => e.Id,
                    c => c.MapFrom(vm => vm.ProgramId)
                );
        }
    }
}