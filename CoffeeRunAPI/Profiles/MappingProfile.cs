using AutoMapper;
using CoffeeRunAPI.Models;
using CoffeeRunAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeRunAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Run, RunViewModel>();
            CreateMap<RunViewModel, Run>();
        }
    }
}
