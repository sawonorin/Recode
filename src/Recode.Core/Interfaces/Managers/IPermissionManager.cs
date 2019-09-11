using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IPermissionManager
    {
        Task<PermissionModel[]> Get();
        Task<bool> AddPermission(PermissionModel[] model);
        Task<bool> Activate(long Id);
        Task<bool> Deactivate(long Id);
        Task<bool> Update(PermissionModel model);
    }
}
