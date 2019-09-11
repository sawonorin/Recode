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
    public class CorporateDocumentRepository : ICorporateDocumentRepository
    {
        private readonly DbContext _dbcontext;
        public CorporateDocumentRepository(DbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<bool> Create(CorporateDocumentModel model)
        {
            _dbcontext.Set<Candidate>().Add(new Candidate
            {
                CompanyId = model.CorporateId,
                FirstName = model.DocumentPath,
                DocumentTypeId = model.DocumentTypeId
            });

            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<CorporateDocumentModel[]> Get(long corporateId)
        {
            return await _dbcontext.Set<Candidate>()
                .Include(d => d.DocumentType)
                .Where(e => e.CompanyId == corporateId)
                .Select(s => new CorporateDocumentModel
                {
                    CorporateId = s.CompanyId,
                    DocumentPath = s.FirstName,
                    DocumentTypeId = s.DocumentTypeId,
                    DocumentType = s.DocumentType.DocumentName
                }).ToArrayAsync();
        }
    }
}
