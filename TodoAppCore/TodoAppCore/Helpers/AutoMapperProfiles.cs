using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppCore.DTOs;
using TodoAppCore.Entities;

namespace TodoAppCore.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Todo, TodoDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.AppUser.UserName));

            CreateMap<TodoDto, Todo>();
        }        
    }
}
