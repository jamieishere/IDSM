using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IDSM.Model;
using IDSM.Repository.DTOs;

namespace IDSM.Repository.AutoMapperConfig
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            ConfigureMapping();
            
        }

        private static void ConfigureMapping()
        {
            Mapper.CreateMap<Game, GameUpdateDTO>();
            //Mapper.CreateMap<UserProfile, UserProfileDTO>();
            //Mapper.CreateMap<Game, GameDTO>();
            //Mapper.CreateMap<Game, GameDTO>();
            //Mapper.CreateMap<Game, GameDTO>();
        } 

    }
}
