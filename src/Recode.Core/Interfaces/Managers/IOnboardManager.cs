using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IOnboardManager
    {
        Task<(bool isSuccessful, string msg)> Onboard(OnboardModel model);
        Task<Page<UserModel>> GetUsers(int pageSize, int pageNumber);
        Task<(UserModel usermodel, bool verifySent)> CreateUserAndAssignToCorporate(UserModel model, long corporateId);
    }
}
