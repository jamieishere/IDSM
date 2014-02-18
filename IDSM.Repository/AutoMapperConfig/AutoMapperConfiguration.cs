﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IDSM.Model;
using IDSM.Repository.DTOs;
using IDSM.ViewModel;

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
            Mapper.CreateMap<Player, PlayerDto>();
        } 
    }
}
