using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IBusinessAccountManager
    {
        Task<BusinessAccountModel[]> GetByCurrentCorporate();
        Task<BusinessAccountModel[]> GetByBusiness(long businessId);
        Task<bool> AddAccount(BusinessAccountModel model);
        Task<bool> Activate(long businessAccountId);
        Task<bool> Deactivate(long businessAccountId);
        Task<bool> Update(BusinessAccountModel model);

        Task<bool> AddFile(FileModel fileModel, long businessId);
        Task<object[]> ByBanks();
        Task<BusinessAccountModel[]> ByBanks(long bankId);
    }
}
