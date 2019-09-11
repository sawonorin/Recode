using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface ICorporateRepository
    {
        Task<bool> AddUserToCorporate(long corporateId, string userId);
        Task<UserCorporateModel> GetUserCorporateByUserId(string userId);
        Task<UserCorporateModel[]> GetUserCorporateByCorporateId(long corporateId);
        Task<UserCorporateModel> Get(string userId, long corporateId);
        Task<Page<UserCorporateModel>> Get(int pageSize, int pageNumber, long corporateId);
    }
}
