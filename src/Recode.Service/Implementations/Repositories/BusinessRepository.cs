using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

namespace Recode.Service.Implementations.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContext _dbcontext;
        public BusinessRepository(DbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbcontext = dbContext;
        }

        public async Task<bool> Activate(long businessId)
        {
            var business = await _dbcontext.Set<Company>().FindAsync(businessId);
            if (business == null)
            {
                return false;
            }

            if (business.IsActive)
            {
                return true;
            }

            business.IsActive = true;
            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> AddUser(string userId, long businessId)
        {
            var business = await _dbcontext.Set<Company>().FindAsync(businessId);

            if (business == null)
            {
                throw new NotFoundException("Business does not exist");
            }

            _dbcontext.Set<UserBusiness>().Add(new UserBusiness
            {
                BusinessId = businessId,
                UserId = userId
            });

            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> AddUser(string userId, long[] businessIds)
        {
            var existingIds = _dbcontext.Set<UserBusiness>()
                .Where(x => businessIds.Any(p => p == x.BusinessId) && x.UserId == userId)
                .Select(x => new UserBusiness
                {
                    BusinessId = x.BusinessId,
                    UserId = x.UserId
                }).Distinct().ToList();

            List<UserBusiness> userBusinesses = businessIds.Select(x => new UserBusiness
            {
                BusinessId = x,
                UserId = userId
            }).ToList();

            var e = userBusinesses.RemoveAll(x => existingIds.Any(p => p.UserId == x.UserId && p.BusinessId == x.BusinessId));

            Log.Info2($"Datas: {JsonConvert.SerializeObject(existingIds)} already exist");

            if (userBusinesses.Count <= 0 && businessIds.Length > 0)
            {
                throw new AlreadyExistException($"User already exist in business(es)");
            }


            _dbcontext.Set<UserBusiness>().AddRange(userBusinesses);

            int count = await _dbcontext.SaveChangesAsync();

            return count > 0;
        }

        public async Task<bool> CreateBusiness(BusinessModel model)
        {
            _dbcontext.Set<Company>().Add(new Company
            {
                Name = model.BusinessCode,
                BusinessName = model.BusinessName,
                CorporateId = model.CorporateId,
                Code = model.Description
            });

            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> Deactivate(long businessId)
        {
            var business = await _dbcontext.Set<Company>().FindAsync(businessId);
            if (business == null)
            {
                return false;
            }

            if (!business.IsActive)
            {
                return true;
            }

            business.IsActive = false;
            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<Page<BusinessModel>> Get(int pageSize, int pageNumber, long corporateId)
        {
            var query = _dbcontext.Set<Company>()
                .Where(x => x.CorporateId == corporateId)
                .Select(x => new BusinessModel
                {
                    Id = x.Id,
                    Description = x.Code,
                    BusinessCode = x.Name,
                    BusinessName = x.BusinessName,
                    CorporateId = x.CorporateId,
                    DateCreated = x.DateCreated,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted
                })
                .OrderByDescending(x => x.DateCreated);

            var result = await query.ToPageListAsync(pageNumber, pageSize);

            return result;
        }

        public async Task<BusinessModel> Get(long businessId, long corporateId)
        {
            return await _dbcontext.Set<Company>().Select(x => new BusinessModel
            {
                Id = x.Id,
                Description = x.Code,
                BusinessCode = x.Name,
                BusinessName = x.BusinessName,
                CorporateId = x.CorporateId,
                DateCreated = x.DateCreated,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            }).FirstOrDefaultAsync(x => x.Id == businessId && x.CorporateId == corporateId);
        }

        public async Task<BusinessModel> Get(string businessName, long corporateId)
        {
            return await _dbcontext.Set<Company>().Select(x => new BusinessModel
            {
                Id = x.Id,
                Description = x.Code,
                BusinessCode = x.Name,
                BusinessName = x.BusinessName,
                CorporateId = x.CorporateId,
                DateCreated = x.DateCreated,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            }).FirstOrDefaultAsync(x => x.BusinessName == businessName && x.CorporateId == corporateId);
        }

        public async Task<bool> UpdateBusiness(BusinessModel model)
        {
            var business = await _dbcontext.Set<Company>().FindAsync(model.Id);
            if (business == null)
            {
                throw new BadRequestException("Business can not be found");
            }

            business.Code = model.Description;
            business.BusinessName = model.BusinessName;
            business.Name = model.BusinessCode;

            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }
    }
}
