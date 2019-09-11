using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Managers
{
    public interface IDocumentTypeManager
    {
        Task<bool> AddDocumentType(DocumentTypeModel model);
        Task<bool> UpdateDocumentType(DocumentTypeModel model);
        Task<DocumentTypeModel[]> Get();
        Task<bool> Activate(long documentId);
        Task<bool> Deactivate(long documentId);
        Task<DocumentTypeModel> Get(long documentId);
    }
}
