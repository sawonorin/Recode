using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IUserService
    {
        Task<ExecutionResponse<UserModelPage>> GetUsers(string email = "", string firstName = "", string lastName = "", string userName = "", long roleId = 0, int pageSize = 10, int pageNo = 1);
        Task<ExecutionResponse<UserModel>> AddUserRole(UserRoleModel model);
        Task<ExecutionResponse<UserModel>> RemoveUserRole(UserRoleModel model);
        Task<ExecutionResponse<UserModel[]>> GetUsers(long[] userIds);
        Task<ExecutionResponse<UserModel>> GetUser(string ssoUserId);
        Task<ExecutionResponse<UserModel>> GetUserByEmail(string email);
        Task<ExecutionResponse<UserModel>> GetUser(long Id);
        Task<ExecutionResponse<UserModel[]>> GetUsersByRoleId(long roleId);
        Task<ExecutionResponse<UserModel>> ToggleActivateUser(long Id);
        Task<ExecutionResponse<UserModel>> OnboardAdmin(OnboardUserModel model);
        Task<ExecutionResponse<UserModel>> CreateUser(CreateUserModel model);
        Task<ExecutionResponse<UserModel>> UpdateUser(UserModel model);
        Task<bool> ResendEmailConfirmation(long UserId);

    }
}
