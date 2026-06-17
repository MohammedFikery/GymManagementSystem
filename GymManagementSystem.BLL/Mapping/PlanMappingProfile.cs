using AutoMapper;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Mapping
{
    public class PlanMappingProfile : Profile
    {
        public PlanMappingProfile()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlaneViewModel>()
                    .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""));
            CreateMap<UpdatePlaneViewModel, Plan>()
                    .ForMember(dest => dest.Name, opt => opt.Ignore())
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}

