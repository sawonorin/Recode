using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IBusinessManager
    {
        Task<bool> CreateBusiness(BusinessModel model);
        Task<bool> UpdateBusiness(BusinessModel model);
        Task<Page<BusinessModel>> Get(int pageSize, int pageNumber);
        Task<bool> Activate(long businessId);
        Task<bool> Deactivate(long businessId);
        Task<bool> AddUser(string userId, long businessId);
        Task<bool> AddUser(string userId, long[] businessId);
    }
}
