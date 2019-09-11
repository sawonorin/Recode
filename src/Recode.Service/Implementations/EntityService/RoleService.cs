using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Recode.Core.Exceptions;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Service.Models;
using Recode.Core.Utilities;
using Recode.Core.ConfigModels;
using System.IO;
using System.Globalization;
using Recode.Service.Utilities;
using Recode.Service.SSO;
using Recode.Repository.CoreRepositories;
using Recode.Data.AppEntity;
using AutoMapper;

namespace Recode.Service.EntityService
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryQuery<Role, long> _roleQueryRepo;
        private readonly IMapper _mapper;

        public RoleService(IRepositoryQuery<Role, long> roleQueryRepo, IMapper mapper)
        {
            _roleQueryRepo = roleQueryRepo;
            _mapper = mapper;
        }

        public async Task<RoleModel[]> GetAll()
        {
            var roles = _roleQueryRepo.GetAll().ToArray();

            return _mapper.Map<RoleModel[]>(roles);
        }
    }
}
