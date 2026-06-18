using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Mapping
{
    public class SessionMappingProfile : Profile
    {
     public SessionMappingProfile()
        {
           CreateMap<Session, SessionViewModel>()
          .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src =>src.Trainer != null? src.Trainer.Name: "N/A"));


            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Trainer, TrainerSelectViewModel>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
