using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<Claim[]> GetClaims(string UserId);
        Task<PermissionModel[]> GetValidPermissionsByIds(long[] ids);
        Task<PermissionModel[]> GetPermissions();
        Task<bool> Activate(long Id);
        Task<bool> Deactivate(long Id);
        Task<bool> Update(PermissionModel model);
        Task<bool> AddPermission(PermissionModel[] models);
        Task<PermissionModel[]> GetPermissions(long roleId);
    }
}
