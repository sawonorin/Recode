using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface IBusinessAccountRepository
    {
        Task<bool> CreateBusinessAccount(BusinessAccountModel model);
        Task<bool> UpdateBusinessAccount(BusinessAccountModel model);
        Task<bool> UpdateMandateFile(long id, string path);
        Task<bool> Activate(long businessAccountId);
        Task<bool> Deactivate(long businessAccountId);
        Task<bool> UpdateBalance(string AccountNumber, long AmountInKobo);
        Task<BusinessAccountModel[]> GetByBusinessId(long businessId);
        Task<Page<BusinessAccountModel>> GetByBusinessId(int pageSize, int pageNumber, long businessId);
        Task<BusinessAccountModel[]> GetByCorporateId(long corporateId);
        Task<Page<BusinessAccountModel>> GetByCorporateId(int pageSize, int pageNumber, long corporateId);
        Task<BusinessAccountModel[]> ByUserId(string userId);
        Task<Page<BusinessAccountModel>> ByUserId(string userId, int pageNumber, int pageSize);
    }
}
