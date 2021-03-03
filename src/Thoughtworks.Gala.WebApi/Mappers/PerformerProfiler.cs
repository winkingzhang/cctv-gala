using AutoMapper;
using Thoughtworks.Gala.WebApi.Entities;
using Thoughtworks.Gala.WebApi.ViewModels;

namespace Thoughtworks.Gala.WebApi.Mappers
{
    public sealed class PerformerProfiler : Profile
    {
        public PerformerProfiler()
        {
            SetupPerformerViewModelToPerformerEntity();
            SetupPerformerViewModelCreationToPerformerEntity();
            SetupPerformerViewModelEditToPerformerEntity();

            SetupPerformerEntityToPerformerViewModel();
        }

        private void SetupPerformerViewModelToPerformerEntity()
        {
            CreateMap<PerformerViewModel, PerformerEntity>()
                .ForMember(
                    e => e.Id,
                    c => c.MapFrom(vm => vm.PerformerId)
                );
        }

        private void SetupPerformerEntityToPerformerViewModel()
        {
            CreateMap<PerformerEntity, PerformerViewModel>()
                .ForMember(
                    vm => vm.PerformerId,
                    c => c.MapFrom(e => e.Id)
                );
        }

        private void SetupPerformerViewModelCreationToPerformerEntity()
        {
            CreateMap<PerformerViewModel.Creation, PerformerEntity>();
        }

        private void SetupPerformerViewModelEditToPerformerEntity()
        {
            CreateMap<PerformerViewModel.Edit, PerformerEntity>()
                .ForMember(
                    e => e.Id,
                    c => c.MapFrom(vm => vm.PerformerId)
                );
        }
    }
}