﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recode.Core.Models;
using Recode.Data.AppEntity;

namespace Recode.Service.AutoMapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserModel>().BeforeMap((src, dest) =>
            {
                if(src.UserRoles != null)
                {
                    dest.Roles = src.UserRoles.Select(x => new RoleModel { Id = x.Role.Id, RoleName = x.Role.RoleName, Description = x.Role.Description }).ToArray();
                }
            });

            CreateMap<Candidate, CandidateModel>().BeforeMap((src, dest) =>
            {
                dest.DepartmentName = src.JobRole?.Department?.Name;
                dest.JobRoleName = src.JobRole?.Name;
            });

            CreateMap<EmailLog, EmailLogModel>().ReverseMap();
        }
    }
}
