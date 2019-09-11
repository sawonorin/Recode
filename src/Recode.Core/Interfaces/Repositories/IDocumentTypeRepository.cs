using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface IDocumentTypeRepository
    {
        Task<bool> AddDocumentType(DocumentTypeModel model);
        Task<bool> UpdateDocumentType(DocumentTypeModel model);
        Task<DocumentTypeModel[]> Get();
        Task<DocumentTypeModel> Get(long documentId);
        Task<bool> Activate(long documentId);
        Task<bool> Deactivate(long documentId);
    }
}
