using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Recode.Data.AppEntity;
using Recode.Core.Interfaces.Repositories;
using Recode.Core.Models;

namespace Recode.Service.Implementations.Repositories
{
    public class EmailRepository: IEmailRepository
    {
        private readonly IMapper _mapper;
        private readonly DbContext _dbcontext;
        public EmailRepository(DbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbcontext = dbContext;
        }

        public async Task<EmailLogModel> CreateMail(EmailLogModel model)
        {
            EmailLog entity = _mapper.Map<EmailLog>(model);
            _dbcontext.Set<EmailLog>().Add(entity);
            int count = await _dbcontext.SaveChangesAsync();
            return _mapper.Map<EmailLogModel>(entity);
        }

        public async Task<EmailLogModel[]> GetUnsentMail(int count)
        {
            var result = await _dbcontext.Set<EmailLog>().Where(x => !x.IsSent).Take(count).ToListAsync();
            if (result.Count > 0)
            {
                return result.Select(x => _mapper.Map<EmailLogModel>(x)).ToArray();
            }
            return Array.Empty<EmailLogModel>();
        }

        public async Task<bool> UpdateMailAfterSent(EmailLogModel model)
        {
            var entity = await _dbcontext.Set<EmailLog>().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity != null)
            {
                entity.DateSent = model.DateSent;
                entity.IsSent = model.IsSent;
                entity.Retires = model.Retires;
            }
            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }
    }
}
