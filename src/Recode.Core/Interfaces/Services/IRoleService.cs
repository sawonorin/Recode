using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<RoleModel[]> GetAll();
    }
}
