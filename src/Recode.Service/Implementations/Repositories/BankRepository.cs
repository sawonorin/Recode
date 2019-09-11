using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigipay.Orbit.Core.Interfaces.Repositories;
using Vigipay.Orbit.Core.Models;
using Recode.Data.AppEntity;

namespace Recode.Service.Implementations.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly DbContext _dbcontext;

        public BankRepository(DbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<BankModel[]> Get()
        {
            return await _dbcontext.Set<Venue>()
                .Select(x => new BankModel
                {
                    BankCode = x.BankCode,
                    Id = x.Id,
                    Name = x.Name,
                    SortCode = x.SortCode
                }).ToArrayAsync();
        }
    }
}
