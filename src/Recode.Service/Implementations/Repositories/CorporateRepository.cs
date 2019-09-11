using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigipay.Orbit.Core.Exceptions;
using Vigipay.Orbit.Core.Interfaces.Repositories;
using Vigipay.Orbit.Core.Interfaces.Services;
using Vigipay.Orbit.Core.Models;
using Recode.Data.AppEntity;
using Recode.Service.Utilities;

namespace Recode.Service.Implementations.Repositories
{
    public class CorporateRepository : ICorporateRepository
    {
        private readonly DbContext _dbcontext;
        private readonly ILoggerService _logService;

        public CorporateRepository(DbContext dbContext, ILoggerService loggerService)
        {
            _dbcontext = dbContext;
            _logService = loggerService;
        }

        public async Task<bool> AddUserToCorporate(long corporateId, string userId)
        {
            var entity = await _dbcontext.Set<User>().FirstOrDefaultAsync(x => x.CompanyId == corporateId && x.SSOUserId == userId);
            if (entity != null)
            {
                if (!entity.IsActive)
                {
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    int count = await _dbcontext.SaveChangesAsync();
                    return count > 0;
                }
                return true;
            }
            _dbcontext.Set<User>().Add(new User
            {
                CompanyId = corporateId,
                SSOUserId = userId
            });
            int result = await _dbcontext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<UserCorporateModel[]> GetUserCorporateByCorporateId(long corporateId)
        {
            var result = await _dbcontext.Set<User>()
                .Where(x => x.CompanyId == corporateId)
                .ToListAsync();

            if (result.Count > 0)
            {
                return result.Select(x => new UserCorporateModel
                {
                    UserId = x.SSOUserId,
                    CorporateId = x.CompanyId,
                    IsActive = x.IsActive
                }).ToArray();
            }

            return Array.Empty<UserCorporateModel>();
        }

        public async Task<UserCorporateModel> GetUserCorporateByUserId(string userId)
        {
            var entity = await _dbcontext.Set<User>().FirstOrDefaultAsync(x => x.SSOUserId == userId);
            if (entity != null)
            {
                return new UserCorporateModel
                {
                    UserId = userId,
                    CorporateId = entity.CompanyId,
                    IsActive = entity.IsActive
                };
            }

            return null;
        }

        public async Task<UserCorporateModel> Get(string userId, long corporateId)
        {
            var x = await _dbcontext.Set<User>().FirstOrDefaultAsync(p => p.CompanyId == corporateId && p.SSOUserId == userId);
            if (x != null)
            {
                return new UserCorporateModel
                {
                    UserId = x.SSOUserId,
                    CorporateId = x.CompanyId,
                    IsActive = x.IsActive
                };
            }
            return null;
        }

        public async Task<Page<UserCorporateModel>> Get(int pageSize, int pageNumber, long corporateId)
        {
            var query = _dbcontext.Set<User>()
                .Where(x => x.CompanyId == corporateId)
                .Select(d => new UserCorporateModel
                {
                    UserId = d.SSOUserId,
                    CorporateId = d.CompanyId,
                    IsActive = d.IsActive,
                    DateCreated = d.DateCreated
                })
                .OrderByDescending(x => x.DateCreated);

            return await query.ToPageListAsync(pageNumber, pageSize);
        }
    }
}
