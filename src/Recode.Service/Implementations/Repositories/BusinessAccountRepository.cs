using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigipay.Orbit.Core.Exceptions;
using Vigipay.Orbit.Core.Interfaces.Repositories;
using Vigipay.Orbit.Core.Models;
using Recode.Data.AppEntity;
using Recode.Service.Utilities;
using Vigipay.Orbit.Core.Utilities;

namespace Recode.Service.Implementations.Repositories
{
    public class BusinessAccountRepository : IBusinessAccountRepository
    {
        private readonly DbContext _dbContext;
        public BusinessAccountRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Activate(long businessAccountId)
        {
            var business = await _dbContext.Set<BusinessAccount>().FindAsync(businessAccountId);
            if (business == null)
            {
                return false;
            }

            if (business.IsActive)
            {
                return true;
            }

            business.IsActive = true;
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> CreateBusinessAccount(BusinessAccountModel model)
        {
            _dbContext.Set<BusinessAccount>().Add(new BusinessAccount
            {
                AccountName = model.AccountName,
                AccountNumber = model.AccountNumber,
                BankId = model.BankId,
                BusinessId = model.BusinessId,
                Comment = model.Comment
            });

            int count = await _dbContext.SaveChangesAsync();

            return count > 0;
        }

        public async Task<bool> Deactivate(long businessBankId)
        {
            var business = await _dbContext.Set<BusinessAccount>().FindAsync(businessBankId);
            if (business == null)
            {
                return false;
            }

            if (!business.IsActive)
            {
                return true;
            }

            business.IsActive = false;
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<BusinessAccountModel[]> GetByBusinessId(long businessId)
        {
            return await _dbContext.Set<BusinessAccount>()
                .Include(s => s.Bank)
                .Include(s => s.Business)
                .Select(x => new BusinessAccountModel
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    BalanceInKobo = x.BalanceInKobo,
                    BankId = x.BankId,
                    BusinessId = x.BusinessId,
                    Comment = x.Comment,
                    VerifiedByUserId = x.VerifiedByUserId,
                    DateVerified = x.DateVerified,
                    IsVerified = x.IsVerified,
                    Id = x.Id,
                    MandateFilePath = x.MandateFilePath,
                    Status = x.Status.ToEnum<BankAccountStatus>(),
                    BankName = x.Bank.Name,
                    DateCreated = x.DateCreated,
                    BusinessName = x.Business.BusinessName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDeleted
                })
                .Where(x => x.BusinessId == businessId)
                .OrderByDescending(r => r.DateCreated)
                .ToArrayAsync();
        }

        public async Task<Page<BusinessAccountModel>> GetByBusinessId(int pageSize, int pageNumber, long businessId)
        {
            var query = _dbContext.Set<BusinessAccount>()
                  .Include(s => s.Bank)
                  .Include(x => x.Business)
                .Select(x => new BusinessAccountModel
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    BalanceInKobo = x.BalanceInKobo,
                    BankId = x.BankId,
                    BusinessId = x.BusinessId,
                    Comment = x.Comment,
                    DateVerified = x.DateVerified,
                    IsVerified = x.IsVerified,
                    Id = x.Id,
                    MandateFilePath = x.MandateFilePath,
                    Status = x.Status.ToEnum<BankAccountStatus>(),
                    BankName = x.Bank.Name,
                    DateCreated = x.DateCreated,
                    BusinessName = x.Business.BusinessName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDeleted
                })
                .Where(x => x.BusinessId == businessId)
                .OrderByDescending(r => r.DateCreated);

            return await query.ToPageListAsync(pageNumber, pageSize);
        }

        public async Task<BusinessAccountModel[]> GetByCorporateId(long corporateId)
        {
            var result = await _dbContext.Set<BusinessAccount>()
                .Include(s => s.Business)
               .Include(s => s.Bank)
               .Where(x => x.Business.CorporateId == corporateId)
                .Select(x => new BusinessAccountModel
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    BalanceInKobo = x.BalanceInKobo,
                    BankId = x.BankId,
                    BusinessId = x.BusinessId,
                    Comment = x.Comment,
                    DateVerified = x.DateVerified,
                    IsVerified = x.IsVerified,
                    Id = x.Id,
                    MandateFilePath = x.MandateFilePath,
                    Status = x.Status.ToEnum<BankAccountStatus>(),
                    BankName = x.Bank.Name,
                    DateCreated = x.DateCreated,
                    BusinessName = x.Business.BusinessName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDeleted
                })
               .OrderByDescending(r => r.DateCreated)
               .ToArrayAsync();

            return result;
        }

        public async Task<Page<BusinessAccountModel>> GetByCorporateId(int pageSize, int pageNumber, long corporateId)
        {
            var query = _dbContext.Set<BusinessAccount>()
                .Include(s => s.Business)
                .Include(s => s.Bank)
               .Where(x => x.Business.CorporateId == corporateId)
                .Select(x => new BusinessAccountModel
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    BalanceInKobo = x.BalanceInKobo,
                    BankId = x.BankId,
                    BusinessId = x.BusinessId,
                    Comment = x.Comment,
                    DateVerified = x.DateVerified,
                    IsVerified = x.IsVerified,
                    Id = x.Id,
                    MandateFilePath = x.MandateFilePath,
                    Status = x.Status.ToEnum<BankAccountStatus>(),
                    BankName = x.Bank.Name,
                    DateCreated = x.DateCreated,
                    BusinessName = x.Business.BusinessName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDeleted
                })
               .OrderByDescending(r => r.DateCreated);

            return await query.ToPageListAsync(pageNumber, pageSize);
        }

        public async Task<bool> UpdateBalance(string AccountNumber, long AmountInKobo)
        {
            var entity = await _dbContext.Set<BusinessAccount>().FirstOrDefaultAsync(x => x.AccountNumber == AccountNumber);
            if (entity == null)
            {
                Log.Info2($"Update Balance: {AccountNumber} does not exist");
                return false;
            }
            entity.BalanceInKobo = AmountInKobo;
            entity.BalanceLastSyncDate = DateTimeOffset.Now;

            int count = await _dbContext.SaveChangesAsync();

            return count > 0;
        }

        public async Task<bool> UpdateBusinessAccount(BusinessAccountModel model)
        {
            var entity = await _dbContext.Set<BusinessAccount>().FindAsync(model.Id);
            if (entity == null)
            {
                throw new BadRequestException("Business account does not exist");
            }

            entity.AccountName = model.AccountName;
            entity.AccountNumber = model.AccountNumber;
            entity.BankId = model.BankId;
            entity.BusinessId = model.BusinessId;
            entity.Comment = model.Comment;

            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> UpdateMandateFile(long id, string path)
        {
            var entity = await _dbContext.Set<BusinessAccount>().FindAsync(id);
            if (entity == null)
            {
                throw new BadRequestException("Business account does not exist");
            }

            entity.MandateFilePath = path;

            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<BusinessAccountModel[]> ByUserId(string userId)
        {
            var userBusiness = await _dbContext.Set<UserBusiness>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            if (userBusiness.Count <= 0)
            {
                return Array.Empty<BusinessAccountModel>();
            }

            return await _dbContext.Set<BusinessAccount>()
                .Include(s => s.Bank)
                .Include(s => s.Business)
                .Select(x => new BusinessAccountModel
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    BalanceInKobo = x.BalanceInKobo,
                    BankId = x.BankId,
                    BusinessId = x.BusinessId,
                    Comment = x.Comment,
                    DateVerified = x.DateVerified,
                    IsVerified = x.IsVerified,
                    Id = x.Id,
                    MandateFilePath = x.MandateFilePath,
                    Status = x.Status.ToEnum<BankAccountStatus>(),
                    BankName = x.Bank.Name,
                    DateCreated = x.DateCreated,
                    BusinessName = x.Business.BusinessName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDeleted
                })
                .Where(x => userBusiness.Any(s => s.BusinessId == x.BusinessId))
                .OrderByDescending(r => r.DateCreated)
                .ToArrayAsync();
        }

        public async Task<Page<BusinessAccountModel>> ByUserId(string userId, int pageNumber, int pageSize)
        {
            var userBusiness = await _dbContext.Set<UserBusiness>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            if (userBusiness.Count <= 0)
            {
                return new Page<BusinessAccountModel>(new BusinessAccountModel[] { }, 0, pageNumber, pageSize);
            }

            var query = _dbContext.Set<BusinessAccount>()
                .Include(s => s.Bank)
                .Include(s => s.Business)
                .Select(x => new BusinessAccountModel
                {
                    AccountName = x.AccountName,
                    AccountNumber = x.AccountNumber,
                    BalanceInKobo = x.BalanceInKobo,
                    BankId = x.BankId,
                    BusinessId = x.BusinessId,
                    Comment = x.Comment,
                    DateVerified = x.DateVerified,
                    IsVerified = x.IsVerified,
                    Id = x.Id,
                    MandateFilePath = x.MandateFilePath,
                    Status = x.Status.ToEnum<BankAccountStatus>(),
                    BankName = x.Bank.Name,
                    DateCreated = x.DateCreated,
                    BusinessName = x.Business.BusinessName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDeleted
                })
                .Where(x => userBusiness.Any(s => s.BusinessId == x.BusinessId))
                .OrderBy(r => r.BankName);

            return await query.ToPageListAsync(pageNumber, pageSize);

        }
    }
}
