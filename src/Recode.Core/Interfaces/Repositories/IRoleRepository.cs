using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<RoleModel> CreateRole(RoleModel model);
        Task<bool> AddUserToRole(long roleId, string userId);
        Task<RoleModel> CreateOrGetRole(RoleModel model);
        Task<bool> ActivateRole(long roleId);
        Task<bool> DeActivateRole(long roleId);
        Task<bool> UpdateRole(RoleModel model);
        Task<Page<RoleModel>> Get(int pageSize, int pageNumber, long coporateId);
        Task<int> AddPermissions(long roleId, long[] permissionIds);
        Task<int> RemovePermissions(long roleId, long[] permissionIds);
        Task<RoleModel> Get(long roleId);
        Task<RoleModel> Get(long roleId, long corporateId);
        Task<RoleModel> GetByCorporate(long corporateId);
        Task<RoleModel[]> GetRoleByUserId(string UserId);
        Task<RoleModel> GetUserRole(string UserId, long roleId);
    }
}
