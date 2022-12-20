using AutoMapper;
using DataAccessLibrary.DTOs;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice16122022.xUnit
{
    public class UserProfiles : Profile
    {
        public UserProfiles() 
        {
            CreateMap<UserModel, ReadUserModelDTO>();
            CreateMap<UserModel, UpdateUserModelDTO>();
            CreateMap<UserModel, CreateUserModelDTO>();
        }
    }
}
