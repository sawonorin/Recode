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

namespace Recode.Service.Implementations.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly DbContext _dbContext;

        public DocumentTypeRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Activate(long documentId)
        {
            var entity = await _dbContext.Set<DocumentType>().FindAsync(documentId);
            if (entity == null)
            {
                throw new BadRequestException("Document can not be found");
            }

            if (entity.IsActive)
            {
                return true;
            }

            entity.IsActive = true;
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> AddDocumentType(DocumentTypeModel model)
        {
            DocumentType entity = new DocumentType()
            {
                DocumentCode = model.DocumentCode,
                DocumentName = model.DocumentName,
                IsRequired = model.IsRequired
            };

            _dbContext.Set<DocumentType>().Add(entity);
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> Deactivate(long documentId)
        {
            var entity = await _dbContext.Set<DocumentType>().FindAsync(documentId);
            if (entity == null)
            {
                throw new BadRequestException("Document can not be found");
            }

            if (!entity.IsActive)
            {
                return true;
            }

            entity.IsActive = false;
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<DocumentTypeModel[]> Get()
        {
            return await _dbContext.Set<DocumentType>()
                .Select(x => new DocumentTypeModel
                {
                    DocumentCode = x.DocumentCode,
                    DocumentName = x.DocumentName,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsRequired = x.IsRequired
                }).OrderByDescending(x => x.DocumentName).ToArrayAsync();
        }

        public async Task<DocumentTypeModel> Get(long documentId)
        {
            var x = await _dbContext.Set<DocumentType>().FindAsync(documentId);
            if (x == null)
            {
                return null;
            }

            return new DocumentTypeModel
            {
                DocumentCode = x.DocumentCode,
                DocumentName = x.DocumentName,
                Id = x.Id,
                IsActive = x.IsActive,
                IsRequired = x.IsRequired
            };
        }

        public async Task<bool> UpdateDocumentType(DocumentTypeModel model)
        {
            var entity = await _dbContext.Set<DocumentType>().FindAsync(model.Id);
            if (entity == null)
            {
                throw new BadRequestException("Document can not be found");
            }

            entity.DocumentCode = model.DocumentCode;
            entity.DocumentName = model.DocumentName;

            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }
    }
}
